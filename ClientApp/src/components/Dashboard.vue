<template>
  <div class="containter-fuild">
    <div
      class="row align-items-center"
      style="
        background-image: linear-gradient(
          135deg,
          rgb(47, 128, 237) 0%,
          rgb(89, 210, 253) 100%
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
          ><img src="../assets/22cat.png" alt="" width="34" height="32" /> Lịch trực</a
        >
      </div>
      <div class="col-sm-3 d-none d-sm-flex align-items-center justify-content-sm-end">
        <a href="/#">Admin</a>
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
      <p>Some contents...</p>
      <p>Some contents...</p>
      <p>Some contents...</p>
    </a-drawer>
</template>

<script>
import { defineComponent, ref } from "vue";
import PopMenu from "./PopMenu.vue";
export default defineComponent({
  components: {
    PopMenu,
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
