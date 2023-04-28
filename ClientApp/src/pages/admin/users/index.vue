<template>
  <div>
    <add-staff @addStaff="handleAddStaff" />
    <staff-list :staffs="staffs" @deleteStaff="handleDeleteStaff" />
  </div>
</template>

<script>
import AddStaff from "./addStaff.vue";
import StaffList from "./staffList.vue";
import axios from "axios";

export default {
  components: {
    "add-staff": AddStaff,
    "staff-list": StaffList,
  },
  data() {
    return {
      staffs: [],
    };
  },
  created() {
    axios
      .get("/api/staff")
      .then((response) => {
        this.staffs = response.data;
      })
      .catch((error) => {
        console.log(error);
      });
  },
  methods: {
    handleAddStaff(staff) {
      this.staffs.push(staff);
    },
    handleDeleteStaff(staffId) {
      axios
        .delete(`/api/staff/${staffId}`)
        .then((response) => {
          this.staffs = this.staffs.filter((s) => s.id !== staffId);
          this.$message.success("Staff deleted successfully");
        })
        .catch((error) => {
          console.log(error);
          this.$message.error("Failed to delete staff");
        });
    },
  },
};
</script>
