<template>
  <div class="form-title text-center">
    <h4 class="title text-center w-100 font-weight-bold">Quản lý lịch trực</h4>
  </div>
  <div class="d-flex flex-column">
    <a-form :model="form" @submit.prevent="login" class="p-1">
      <a-form-item
        label="Username"
        name="account"
        :rules="[{ required: true, message: 'Please input your username' }]"
      >
        <a-input v-model.trim="form.account" />
      </a-form-item>
      <a-form-item
        label="Password"
        name="password"
        :rules="[{ required: true, message: 'Please input your password' }]"
      >
        <a-input v-model.trim="form.password" type="password" />
      </a-form-item>

      <a-form-item>
        <a-button
          class="btn btn-info btn-block btn-round"
          :loading="loading"
          html-type="submit"
          >Login</a-button
        >
        <a-alert
          class="error"
          type="error"
          :show-icon="true"
          :show="error !== null"
          :closable="true"
          @close="error = null"
          >{{ error }}</a-alert
        >
      </a-form-item>
    </a-form>
  </div>
</template>

<script>
import { ref } from "vue";
import { Form, Input, Button, Alert } from "ant-design-vue";

export default {
  components: {
    "a-form": Form,
    "a-form-item": Form.Item,
    "a-input": Input,
    "a-button": Button,
    "a-alert": Alert,
  },
  setup() {
    const loading = ref(false);
    const form = ref({
      account: "",
      password: "",
    });
    const error = ref(null);

    const login = () => {
      form.value.$validate(async (valid) => {
        if (valid) {
          loading.value = true;
          try {
            // Gọi API để xác thực tài khoản và mật khẩu
            const response = await fetch("/api/authentication/staff", {
              method: "POST",
              headers: { "Content-Type": "application/json" },
              body: JSON.stringify({
                account: form.value.account,
                password: form.value.password,
              }),
            });
            const data = await response.json();
            if (data.success) {
              // Nếu xác thực thành công, chuyển hướng đến trang chính
              window.location.href = "/admin";
            } else {
              // Nếu xác thực thất bại, hiển thị thông báo lỗi
              error.value = data.message;
            }
          } catch (err) {
            // Xử lý lỗi nếu có
            console.error(err);
            error.value = "An error occurred. Please try again later.";
          } finally {
            loading.value = false;
          }
        }
      });
    };

    return {
      loading,
      form,
      error,
      login,
    };
  },
};
</script>
<style scoped>
.login-form {
  max-width: 300px;
  margin: auto;
  padding-top: 50px;
}

.login-form-forgot {
  float: right;
}

.login-form-button {
  width: 100%;
}
</style>
