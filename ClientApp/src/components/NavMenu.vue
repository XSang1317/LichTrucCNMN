<template>
  <div class="containter-fuild">
    <div
      class="row align-items-center"
      style="
        background-image: linear-gradient(
          135deg,
          rgb(5, 97, 127) 0%,
          rgb(36, 121, 151) 100%
        );
        padding: 1rem;
      "
    >
      <div class="col-1 d-flex d-sm-none align-items-center justify-content-center">
        <span @click="showDrawer()">
          <i class="fa-solid fa-bars" />
        </span>
      </div>

      <div
        class="col-10 col-sm-9 d-flex align-items-center justify-content-center justify-content-sm-start"
        to="/"
      >
        <a href="/"
          ><img src="../assets/111.png" alt="" width="34" height="32" /> Lịch trực</a
        >
      </div>
      <!-- <div class="col-sm-3 d-none d-sm-flex align-items-center justify-content-sm-end">
        <a href="/login">Login</a>
      </div> -->
      <div class="col-sm-3 d-none d-sm-flex align-items-center justify-content-sm-end">
          <Login></Login>
      </div>
      <div class="col-1 d-flex d-sm-none align-items-center justify-content-center">
        <span @click="showDrawerUser()"><i class="fa-regular fa-user"></i></span>
      </div>
    </div>
  </div>
  <a-drawer v-model:visible="visible" title="Danh mục" placement="left">
    <PopMenu />
  </a-drawer>
  <a-drawer v-model:visible="visible_user" title="Admin" placement="right">
   <Login/>
  </a-drawer>
</template>

<script>
import PopMenu from "./PopMenu.vue";
import Login from "./Login/Login.vue";
import { defineComponent, ref } from "vue";
export default defineComponent({
  components: {
    PopMenu,
    Login,
  },
  setup() {
    const visible = ref(false);
    const visible_user = ref(false);
    const showDrawer = () => {
      visible.value = true;
    };
    const showDrawerUser = () => {
      visible_user.value = true;
    };
    return {
      visible,
      visible_user,
      showDrawer,
      showDrawerUser,
    };
  },
});
</script>
<style scoped>
a {
  text-decoration: none;
  color: white;
  font-weight: 700;
  font-size: larger;
}
.read-the-docs {
  color: #888;
}
</style>
