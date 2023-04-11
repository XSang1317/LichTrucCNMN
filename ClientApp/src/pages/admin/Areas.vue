<template>Đây là Trang quản lý Areas</template>
<script>
import { useMenu } from "../../stores/user-menu";
export default {
  setup() {
    useMenu().onSelectedKeys(["admin-areas"]);
  },
};
</script>
