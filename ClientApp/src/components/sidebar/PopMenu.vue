<template>
  <a-list>
    <a-menu v-model:openKeys="openKeys" v-model:selectedKeys="selectedKeys" mode="inline">
      <a-menu-item key="admin-staffs">
        <router-link :to="{ name: 'admin-staffs' }">
          <span> <i class="fa-solid fa-user-gear me-1"></i> Nhân viên </span>
        </router-link>
      </a-menu-item>

      <a-menu-item key="admin-roles">
        <router-link :to="{ name: 'admin-roles' }">
          <span><i class="fa-solid fa-tags"></i> Thêm quyền </span>
        </router-link>
      </a-menu-item>

      <a-menu-item key="admin-permissions">
        <router-link :to="{ name: 'admin-permissions' }">
          <span><i class="fa-solid fa-tags"></i> Phân quyền </span>
        </router-link>
      </a-menu-item>

      <a-menu-item key="admin-areas">
        <router-link :to="{ name: 'admin-areas' }">
          <span><i class="fa-solid fa-warehouse me-1"></i>Khu vực</span>
        </router-link>
      </a-menu-item>
      <!--    <a-sub-menu key="admin-areas" title="Submenu">
          <a-menu-item key="areas-HC">Hành Chính</a-menu-item>
          <a-menu-item key="areas-SF">Phần mềm</a-menu-item>
        </a-sub-menu> -->
      <a-menu-item key="admin-shifts">
        <router-link :to="{ name: 'admin-shifts' }">
          <span><i class="fa-regular fa-calendar-plus"></i> Ca trực </span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="admin-type">
        <router-link :to="{ name: 'admin-type' }">
          <span><i class="fa-solid fa-hourglass"></i> Loại Ca </span>
        </router-link>
      </a-menu-item>
    </a-menu>
  </a-list>
</template>

<script>
import {} from "@ant-design/icons-vue";
import { defineComponent } from "vue";
import { storeToRefs } from "pinia";
import { useMenu } from "../../stores/user-menu";
export default defineComponent({
  components: {},
  setup() {
    const store = useMenu();

    return {
      ...storeToRefs(store),
    };
  },
});
</script>
<style>
a {
  color: black;
  text-decoration: none;
}
</style>
