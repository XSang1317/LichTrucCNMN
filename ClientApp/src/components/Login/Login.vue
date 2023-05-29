<template>
  <div>
    <a-modal
      :visible="visible"
      @ok="login"
      @cancel="handleCancel"
      width="50%"
    >
      <a-row type="flex" justify="center" align="middle" class="min-vh-250">
        <a-col class="row  rounded-5 p-3 bg-white box-area">
          <a-card class="box-area">
            <a-row type="flex" align="middle">
              <a-col :span="12">
                <div class="featured-image mb-3">
                  <img src="./../../assets/fw.png" class="img-fluid" style="width: 300px" />
                </div>
              </a-col>
              <a-col :span="12">
                <a-row type="flex" justify="center" align="middle">
                  <a-col :span="24">
                    <div class="header-text mb-4">
                      <h2 style="color: #deb438"><img src="../../assets/logo.png" style="width: 80px;"/>Xin chào</h2>
                    </div>
                    <a-form :form="form2" class="login-form">
                      <a-form-item>
                        <a-input v-model:value="form2.username" placeholder="Tên tài khoản" />
                      </a-form-item>
                      <a-form-item>
                        <a-input-password
                          v-model:value="form2.password"
                          placeholder="Mật khẩu"
                        />
                      </a-form-item>
                      <a-form-item>
                        <a-checkbox v-model:checked="rememberMe">Ghi nhớ</a-checkbox>
                        <a href="#" class="forgot">Bạn quên mật khẩu?</a>
                      </a-form-item>
                     <!--  <a-form-item>
                        <a-button type="primary" class="login-button" @click="login()"
                          >Đăng nhập</a-button
                        >
                      </a-form-item> -->
                    </a-form>
                  </a-col>
                </a-row>
              </a-col>
            </a-row>
          </a-card>
        </a-col>
      </a-row>
    </a-modal>
    <a-button type="primary" @click="showModal" shape="round"
      ><i class="fa-solid fa-house" />Login</a-button
    >
  </div>
</template>

<script>
import axios from "axios";
import { Modal, Form, Input, Select, message } from "ant-design-vue";

export default {
  components: {
    "a-modal": Modal,
    "a-form": Form,
    "a-form-item": Form.Item,
    "a-input": Input,
    "a-select": Select,
    "a-select-option": Select.Option,
    "a-input-password": Input.Password,
    message,
  },
  data() {
    return {
      visible: false,
      form2: {
        username: "",
        password: "",
      },
    };
  },
  methods: {
    showModal() {
      this.visible = true;
    },
    handleCancel() {
      this.visible = false;
      this.form2.username = "";
      this.form2.password = "";
    },
    login() {
      axios
        .post("/api/authentication/staff", this.form2)
        .then((response) => {
          this.visible = false;
          this.$emit("login", response.data);
          message.success("Đăng nhập thành công");
          this.$router.push("/admin");
        })
        .catch((error) => {
          console.log(error);
          message.error("Đăng nhập thất bại");
        });
    },
  },
};
</script>
<style>
@import url("https://fonts.googleapis.com/css2?family=Poppins:wght@400;500&display=swap");

body {
  background: #ececec;
}
p {
    display: block;
    margin-block-start: 1em;
    margin-block-end: 1em;
    margin-inline-start: 0px;
    margin-inline-end: 0px;
}
.bg-white {
    --bs-bg-opacity: 1;
    background-color: rgba(var(--bs-white-rgb),var(--bs-bg-opacity))!important;
}
.row {
    --bs-gutter-x: 1.5rem;
    --bs-gutter-y: 0;
    display: flex;
    flex-wrap: wrap;
    margin-top: calc(-1 * var(--bs-gutter-y));
    margin-right: calc(-.5 * var(--bs-gutter-x));
    margin-left: calc(-.5 * var(--bs-gutter-x));
}
*,
::after,
::before {
  box-sizing: border-box;
}

/*------------ Login container ------------*/

.box-area {
  width: 100%;
}
.img-fluid {
    max-width: 100%;
    height: auto;
}

/*------------ Right box ------------*/

.right-box {
  padding: 40px 30px 40px 40px;
}

/*------------ Custom Placeholder ------------*/

::placeholder {
  font-size: 16px;
}

.rounded-4 {
  border-radius: 20px;
}
.rounded-5 {
  border-radius: 30px;
}
.bg-white {
    --bs-bg-opacity: 1;
    background-color: rgba(var(--bs-white-rgb),var(--bs-bg-opacity))!important;
}
.p-3 {
    padding: 1rem!important;
}
/* .border {
    border: 1px solid #dee2e6!important;
} */
.row {
    --bs-gutter-x: 1.5rem;
    --bs-gutter-y: 0;
    display: flex;
    flex-wrap: wrap;
    margin-top: calc(-1 * var(--bs-gutter-y));
    margin-right: calc(-.5 * var(--bs-gutter-x));
    margin-left: calc(-.5 * var(--bs-gutter-x));
}
*, ::after, ::before {
    box-sizing: border-box;
}

/*------------ For small screens------------*/

@media only screen and (max-width: 768px) {
  .box-area {
    margin: 0 10px;
  }
  .left-box {
    height: 100px;
    overflow: hidden;
  }
  .right-box {
    padding: 20px;
  }
}
.btn-group-lg > .btn,
.btn-lg {
  padding: 0.5rem 1rem;
  font-size: 1.25rem;
  border-radius: 4.3rem;
}
@media (min-width: 1200px) {
  .fs-2 {
    font-size: 2rem !important;
  }
}
/*------------ Antdv CSS------------*/
.ant-col-12 {
  display: inline;
  flex: 0 0 50%;
  max-width: 50%;
}
.ant-modal-body {
    /* padding: 24px; */
    font-size: 14px;
    line-height: 1.5715;
    word-wrap: break-word;
}
</style>
