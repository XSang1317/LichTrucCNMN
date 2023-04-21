<template>
  <div class="form-title text-center">
    <h4 class="title text-center w-100 font-weight-bold">Quản lý lịch trực</h4>
  </div>
  <div class="d-flex flex-column">
    <form @submit.prevent="login" class="p-1">
      <a-form class="form-group" label="Username">
        <a-input v-model="Username" />
      </a-form>
      <a-form class="form-group" label="Password">
        <a-input v-model="password" type="password" />
      </a-form>

      <a-form>
        <a-button
          class="btn btn-info btn-block btn-round"
          pill
          type="primary"
          :disabled="loading"
          >Login</a-button
        >
        <!-- style="float: left; margin-left: 180px"> -->
        <!-- <b-button variant="default" @click="close" :disabled="loading">Thoát</b-button> -->
        <a-alert
          message="Error"
          type="error"
          show-icon
          dismissible
          @dismissed="error = null"
          >{{ error }}</a-alert
        >
      </a-form>
    </form>
  </div>

</template>

<script>
import axios from "axios";
export default {
  name: "auth-modal",
  prop: {
    show: { type: Boolean, required: true },
  },
  data() {
    return {
      Username: "",
      email: "",
      password: "",
      error: null,
      loading: false,
    };
  },
  methods: {
    login() {
      this.loading = true;
      const payload = {

        username: this.Username,
        password: this.password,
      };
      axios
        .post("/api/authetication/Staffs", payload)

        .then((response) => {
          const auth = response.data;
          this.$log(auth.status);
          this.$log(response.data);
          this.$log(response);
          axios.defaults.headers.common["Authorization"] = `Bearer ${auth.accessToken}`;
          this.$store.commit("loginSuccess", auth);
          this.error = null;

          this.Username = "";

          this.email = "";
          this.password = "";
          this.loading = false;

          if (this.$route.query.redirect) {
            this.$route.push(this.$route.query.redirect);
          } else {
            this.$route.push("/");
          }
        })
        .catch((error) => {
          this.loading = false;
          delete axios.defaults.headers.common["Authorization"];
          this.error = error.response.data;
        });
    },
  },
};
</script>
