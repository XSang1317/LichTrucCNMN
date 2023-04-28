<template>
  <div>
    <a-button type="primary">Shifts</a-button>
  </div>
</template>
<script>
import { useMenu } from "../../stores/user-menu";
export default {
  setup() {
    useMenu().onSelectedKeys(["admin-shifts"]);
  },
};
</script>
