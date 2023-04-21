<template>
  <div id="content-wrapper">
    <div class="container-fluid">
      <a-row>
        <a-col cols="12">
          <div class="form-inline">
            <a-button
              class="btn-sm border-0 btn-info mb-1 mr-2"
              style="float: right"
              type="primary"
              shape="round"
              @click.prevent="openCreateModal"
            >
              <span><i class="fa-solid fa-user-plus" /> Add User</span>
            </a-button>
          </div>
        </a-col>
        <a-col cols="12">
          <div class="form-inline">
            <a-input-search
              class="mb-2 mr-2 h-auto pt-0 d-sm"
              style="flex-grow: 1; width: 100px"
              v-model="query"
              placeholder="Search by Username, Email or Full Name"
              @keydown.enter.prevent="queryItem"
              @click.prevent="queryItem"
            />
            <!--  <a-button
              class="btn-sm border-0 btn-outline-secondary mb-2"
              style="float: right"
              @click.prevent="queryItem"
            >
              <i class="fas fa-search"></i>
            </a-button> -->
          </div>
        </a-col>
      </a-row>

      <a-table
        :aria-colcount="colums"
        :datas-source="dataSource"
        hover
        responsive
        :aria-sort="username"
        bordered
        :fields="staff_fields"
        :items="staff_items"
        class-name="table-light"
      >
        <template v-slot:cell(username)="data">
          {{ data.value }}
        </template>

        <template v-slot:cell(area)="data">
          <div class="text-center">
            {{ findPickList(area_items, data.item.area) }}
          </div>
        </template>
        <template v-slot:cell(status)="data">
          <div class="text-center">
            <a-badge v-if="data.item.status == 1" color="green"> Active </a-badge>
            <a-badge v-else color="red"> Deactive </a-badge>
          </div>
        </template>
      </a-table>
    </div>
  </div>
  <a-modal
    content-class="border-0 p-0"
    v-model="creatModelShow"
    aria-setsize="m"
    :maskClosable="false"
    hide-footer
  >
    <template v-slot:modal-title> </template>
    <a-form aria-autocomplete="off" @keydown.enter.prevent="submitCreate">
      <a-row>
        <a-col>
          <a-form>
            <template v-slot:label> Username </template>
            <a-input
              v-model="staff.username"
              class="h-auto pt-0 pb-0"
              oninvalid="this.SetCustiomValidity('Please enter Username')"
              oninput="setCustomValidity('')"
              aria-required
            />
          </a-form>
        </a-col>
      </a-row>
      <a-row>
        <a-col>
          <a-form>
            <a-form-item></a-form-item>
          </a-form>
        </a-col>
      </a-row>
    </a-form>
  </a-modal>

</template>
<script>
import { useMenu } from "../../stores/user-menu";
import axios from "axios";
export default {
  computed: {
  name: "Staffs",
  /* computed: {

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

      uploadImage: false,
      isImageUploading: false,

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

          key:"status",
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

  }, */

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
        .put("/api/staff", this.staff)
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
};}
</script>
