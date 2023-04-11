<template>
  <Dashboard/>
  <div class="container-fluid mt-3">
    <div class="row">
      <div class="col-sm-3 d-none d-sm-flex">
        <a-list bordered style="width: 100%">
          <PopMenu />
          <template #header>
            <h4 style="margin: 8px, font-family:arial">Quản lý</h4>
          </template>
        </a-list>
      </div>

      <div class="col-12 col-sm-9">
        <router-view></router-view>
      </div>
    </div>
  </div>
</template>
<script>
import PopMenu from "../../components/PopMenu.vue";
import Dashboard from "../../components/Dashboard.vue";
export default {
  components: {
    PopMenu,
    Dashboard,
  },
};
</script>
<style scoped>
a {
  text-decoration: none;
}
</style>
