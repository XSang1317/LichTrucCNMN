<template>
  <a-card title="Nhân viên" style="100%">
    <div class="row">
      <div class="col-12">
        <a-table :dataSource="dataSource" :columns="columns"/>
      </div>
    </div>
  </a-card>
</template>
<script>
import { useMenu } from "../../stores/user-menu";
import axios from "axios";
export default {
  name: "Users",
  components: {
    ////////////////////////////////
  },
  computed: {
    auth() {
      return this.$store.state.auth;
    },
    hethong_insert() {
      return this.$store.getters.hasPermission("hethong_insert");
    },
    hethong_update() {
      return this.$store.getters.hasPermission("hethong_update");
    },
    hethong_delete() {
      return this.$store.getters.hasPermission("hethong_delete");
    },
    crud_user() {
      return this.$store.getters.hasPermission("crud_user");
    },
  },
  data() {
    return {
      isDisabled: false,
      staff_flush: null,
      query: null,
      creatModelShow: false,
      editModalShow: false,

      staff_fields: [
        {
          key: "usernames",
          lable: "usernames",
          sorttbale: true,
          thStyle: {
            width: "140px !importand",
            minWidth: "140px !importand",
            maxWidth: "140px !importand",
          },
        },
        {
          key: "email",
          lable: "email",
          sorttbale: true,
          thStyle: { with: "300px !importand" },
        },
        {
          key: "name",
          lable: "FullName",
          sorttbale: true,
          thStyle: { with: "300px !importand" },
        },
        {
          key: "Phone",
          lable: "Phone",
          sorttbale: true,
          thStyle: { with: "100px !importand" },
        },
        {
          key: "role_name",
          lable: "Role",
          sorttbale: true,
          thStyle: { with: "100px !importand" },
        },
        {
          key: "status",
          lable: "Status",
          sorttbale: true,
          thStyle: {
            width: "80px !important",
            minWidth: "80px !important",
            maxWidth: "80px !important",
          },
        },
        {
          key: "action",
          label: "",
          class: "text-center",
          thStyle: { width: "50px !important", minWidth: "50px !important" },
        },
      ],
      staff_items: [],
      currentIndex: -1,
      user: {
        id: 0,
        username: "",
        name: "",
        email: "",
        phone: "",
        password: "",
        //area:"", -------------- Add name area
        repassword: "",
        role_name: "",
        status: 1,
        //signature:"",
      },
    };
  },
  beforeRouteEnter(to, from, next) {
    axios.all([axios.get(`/api/staff`, { params: to.query })]).then(
      axios.spread((staff, area) => {
        next((vn) => vn.initData(staff.data));
      })
    );
  },
  beforeRouteUpdate(to, from, next) {
    axios
      .get("/api/staff", { params: to.query })
      .then((staff) => {
        this.staff_items = staff.data;
        next();
      })
      .catch((err) => {
        next();
      });
  },
  methods: {
    initData(staff, area) {
      this.staff_items = staff;
      this.area_items = area;
      this.staff_flush = this.$copy(this.staff);
    },

    /* Create Method */
    openCreateModal() {
      this.staff.id = 0;
      this.staff.username = "";
      this.staff.name = "";
      this.staff.phone = "";
      this.staff.email = "";
      this.staff.area = 0;
      this.staff.password = "";
      this.staff.repassword = "";
      this.staff.role_name = "";
      this.staff.status = "";
      this.creatModelShow = true;
    },
    closeCreateModal() {
      this.creatModelShow = false;
    },
    submitCreate() {
      this.isDisabled = true;
      if (!this.$email_format_regex.test(this.staff.email)) {
        this.$error_message(
          "Please enter a valid email address in format: yourname@example.com"
        );
        return (this.isDisabled = false);
      }
      if (!this.$phone_format_regex.test(this.staff.phone)) {
        this.$error_message(
          "Please enter a valid phone number in format: (84)000-000-0000!"
        );
        return (this.isDisabled = false);
      }
      if (this.staff.password != this.staff.repassword) {
        this.$error_message("Passwords do not match");
        return (this.isDisabled = false);
      }
      this.staff.status = this.staff.status == "1" ? 1 : 0;
      this.staff.CreatedBy = this.$store.state.auth.usernameId;
      axios
        .post("api/staff", this.staff)
        .then((res) => {
          this.staff_items.unshift(res.data);
          this.$success_message("Your account has been created!");
          this.isDisabled = false;
          this.closeCreateModal();
        })
        .catch((err) => {
          this.isDisabled = false;
          this.$error_message(err.responsive.data);
        });
    },
    /* END CREATED MOTHOD */

    /* EDIT METHOD */
    openEditModa(data) {
      this.currentIndex = this.staff_items.indexOf(data.item);
      axios
        .get(`api/staff/${data.item.username}`)
        .then((res) => {
          this.staff.id = data.item.id;
          this.staff.username = data.item.username;
          this.staff.name = data.item.name;
          this.staff.email = data.item.email;
          this.staf.phone = data.item.phone;
          this.staff.status = data.item.status;
          this.staff.area = data.item.area;

          this.editModalShow = true;
        })
        .catch((err) => {
          this.$toast.error(err.response.data, "Error", {
            timeout: 3000,
          });
        });
    },
    closeEditModal() {
      this.editModalShow = false;
    },
    submitEdit() {
      this.isDisabled = true;
      if (!this.$emai_format_regex.test(this.staff.email)) {
        this.$error_message(
          "Please enter a valid email address in format: yourname@example.com "
        );
        return (this.isDisabled = false);
      }
      if (this.staff.phone != "" && this.staff.phone != null) {
        if (!this.$phone_format_regex.test(this.staff.phone)) {
          this.$error_message(
            "Please enter a valid phone number in format: (84) 000-000-0000"
          );
          return (this.isDisabled = false);
        }
      }
      this.staff.status = this.staff.status == "1" ? 1 : 0;
      this.staff.UpdatedBy = this.$store.state.auth.usernameId;
      axios
        .put("/api/Staffs", this.staff)
        .then((res) => {
          const staff_edit = this.$copy(this.staff);
          this.staff_items.splice(this.currentIndex, 1, staff_edit);

          this.$success_message("Your account has been updated");
          this.isDisabled = false;
          this.closeEditModal();
        })
        .catch((err) => {
          this.isDisabled = false;
          this.$error_message(err.response.data);
        });
    },
    /*END EDIT METHOD*/
  },
  setup() {
    useMenu().onSelectedKeys(["admin-staffs"]);
  },
};
</script>
