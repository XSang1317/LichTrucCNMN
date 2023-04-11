<template>
  <section class="vh-10">
    <div class="container py-5 h-100">
      <div class="row d-flex align-items-center justify-content-center h-100">
        <H4>Quản Lý Lịch Trực</H4>
        <div class="col-md-8 col-lg-7 col-xl-6">
          <img
            src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/draw2.svg"
            class="img-fluid"
            alt="Phone image"
          />
        </div>
        <div class="col-md-7 col-lg-5 col-xl-5 offset-xl-1">
          <form>
            <!-- Email input -->
            <div class="input">
              <label for="email">Email address</label>
              <input
                type="email"
                id="email"
                placeholder="email@address.com"
                class="form-control form-control-lg"
              />
            </div>

            <!-- Password input -->
            <div class="input">
              <label class="form-label" for="form1Example23">Password</label>
              <input
                type="password"
                id="form1Example23"
                class="form-control form-control-lg"
              />
            </div>

            <div class="d-flex justify-content-around align-items-center mb-4">
              <!-- Checkbox -->
              <div class="form-check">
                <label class="form-check-label" for="form1Example3"> Remember me </label>
                <input
                  class="form-check-input"
                  type="checkbox"
                  value=""
                  id="form1Example3"
                  checked
                />
              </div>
              <a href="#!">Forgot password?</a>
            </div>
            <p>
              Don't have an account?
              <a href="/register" class="link-info">Register here</a>
            </p>
            <!-- Submit button -->
            <button type="submit" class="mt-4 btn btn-primary" @click="goToDashboard">
              Login
            </button>
          </form>
        </div>
      </div>
    </div>
  </section>
</template>
<script>
import { defineComponent, reactive } from "vue";
//import axios from "axios";
export default defineComponent({
  /*   data() {
      return {
        username: "",
        password: "",
        email: "",
        error: null,
        remember: true,
        loading: false,
      };
    },
    methods: {
      login() {
        this.loading = true;
        const payload = {
          username: this.username,
          password: this.password,
        };
        axios.post("api/Staffs/");
      },
    }, */
  methods: {
    goToDashboard() {
      this.$router.push("/admin");
    },
    moveToRegister() {
      this.$router.push("/register");
    },
  },
  setup() {
    const formState = reactive({
      username: "",
      password: "",
      remember: true,
    });
    const onFinish = (values) => {
      console.log("Success:", values);
    };
    const onFinishFailed = (errorInfo) => {
      console.log("Failed:", errorInfo);
    };
    return {
      formState,
      onFinish,
      onFinishFailed,
    };
  },
});
</script>
<style>
.divider:after,
.divider:before {
  content: "";
  flex: 1;
  height: 1px;
  background: #eee;
}
a {
  text-decoration: none;
}
</style>
