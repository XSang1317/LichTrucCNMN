<template>
  <div id="wrapper">
    <nhan-su-side-bar index="2"></nhan-su-side-bar>

    <div id="content-wrapper">
      <div class="container-fluid">
        <div class="row">
          <div class="col-4 pr-1">
            <form
              @submit.prevent="addItem"
              class="form-inline"
              style="height: 40px; max-height: 40px"
            >
              <b-form-input
                class="mr-2 h-auto pt-0 pb-0"
                style="flex-grow: 1; width: 1px"
                v-model="added_item_name"
                placeholder="Group Name"
              />
              <button class="btn-sm border-0 btn-info" type="submit">
                <i class="fas fa-plus"></i> Add Group
              </button>
            </form>
            <b-table
              style
              borderless
              small
              striped
              id="my-table"
              head-variant="dark"
              hover
              responsive
              :fields="fields"
              :items="items"
              selectable
              thead-class="text-center"
              select-mode="single"
              @row-selected="onRowSelected"
              table-variant="light"
            >
              <template v-slot:cell(selected)="{ rowSelected }">
                <div class="text-center">
                  <template v-if="rowSelected">
                    <span aria-hidden="true">&check;</span>
                    <span class="sr-only">Selected</span>
                  </template>
                  <template v-else>
                    <span aria-hidden="true">&nbsp;</span>
                    <span class="sr-only">Not selected</span>
                  </template>
                </div>
              </template>
              <template #top-row="">
                <b-td></b-td>
                <b-td
                  ><input
                    class="form-control h-auto pt-0 pb-0"
                    v-model="groupSearchQuery"
                    placeholder="Search Group by name"
                    @keydown.enter.prevent="groupSearchqueryItem"
                  />
                </b-td>
                <b-td class="text-center"
                  ><button
                    class="btn-sm border-0 btn-outline-secondary"
                    @click.prevent="groupSearchqueryItem"
                  >
                    <i class="fas fa-search"></i></button
                ></b-td>
              </template>

              <template v-slot:cell(name)="data">
                <input
                  type="text"
                  class="form-control h-auto pt-0 pb-0"
                  :value="data.item.name"
                  required
                  @change="updateItem(data, $event)"
                />
              </template>
              <template v-slot:cell(deleted)="data">
                <b-link
                  class="btn-sm border-0 btn-outline-danger"
                  href="#"
                  @click.prevent="deleteItem(data)"
                >
                  <i class="fas fa-trash"></i>
                </b-link>
              </template>
            </b-table>
          </div>
          <div class="col-8 pl-1">
            <form
              @submit.prevent="openSearchModal"
              class="form-inline"
              style="height: 40px; max-height: 40px"
            >
              <button
                v-show="item_selected != null"
                class="btn-sm border-0 btn-info mb-2 mt-auto ml-auto"
                type="submit"
              >
                <i class="fas fa-plus"></i> Add User To Group
              </button>
            </form>
            <b-table
              borderless
              small
              striped
              id="my-table"
              head-variant="dark"
              hover
              thead-class="text-center"
              responsive
              :fields="fieldsUser"
              :items="users"
              table-variant="light"
            >
              <template #top-row="">
                <b-td colspan="3"
                  ><input
                    class="form-control h-auto pt-0 pb-0"
                    v-model="userSearchQuery"
                    placeholder="Search User by Username, Email or Full Name"
                    @keydown.enter.prevent="findUserInRole(userSearchQuery)"
                  />
                </b-td>
                <b-td class="text-center"
                  ><button
                    class="btn-sm border-0 btn-outline-secondary"
                    @click.prevent="findUserInRole(userSearchQuery)"
                  >
                    <i class="fas fa-search"></i></button
                ></b-td>
              </template>
              <!-- <template v-slot:cell(deleted)="data" v-if="hethong_delete"> -->
              <template v-slot:cell(deleted)="data">
                <b-link
                  class="btn-sm border-0 btn-outline-danger"
                  href="#"
                  @click.prevent="deleteUser(data)"
                >
                  <i class="fas fa-trash"></i>
                </b-link>
                <!-- <button class="btn btn-danger" @click="deleteUser(data)">
                    <i class="fas fa-trash-alt"></i>
                  </button> -->
              </template>
            </b-table>
          </div>
        </div>
      </div>
      <!-- /.container-fluid -->
      <!-- SEARCH MODAL -->
      <b-modal
        v-model="searchModalShow"
        size="lg"
        content-class="p-0"
        @hidden="closeSearchModal"
        :no-close-on-backdrop="true"
        :no-close-on-esc="true"
        header-bg-variant="info"
        header-text-variant="light"
        hide-footer
      >
        <template v-slot:modal-title> Add User To Group </template>
        <b-form
          @submit.prevent="addUser"
          @keydown.enter.prevent="addUser"
          class="form-inline mb-3"
          v-show="item_selected != null"
        >
          <div class="form-group mr-auto demo col-auto">
            <vue-autosuggest
              v-model="userQuery"
              :suggestions="usersuggestions"
              :input-props="{
                id: 'autosuggest__input',
                placeholder: 'Search by Username, Email or Full Name',
              }"
              @selected="onSelected"
              @input="onInputChange"
              :get-suggestion-value="getSuggestionValue"
            >
              <template
                slot-scope="{ suggestion }"
                style="display: flex; align-items: center"
              >
                <span class="my-suggestion-item mr-3"
                  >{{ suggestion.item.hovaten }}
                </span>
              </template>
            </vue-autosuggest>
          </div>
          <div class="form-group mt-1 mb-auto">
            <button class="btn btn-info" type="submit">
              <i class="fas fa-plus"></i> Add User
            </button>
          </div>
        </b-form>
      </b-modal>
      <!-- END OF SEARCH MODAL -->
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { VueAutosuggest } from "vue-autosuggest";
import NhanSuSideBar from "../components/NhanSu/NhanSuSideBar";
export default {
  name: "Roles",
  components: {
    VueAutosuggest,
    NhanSuSideBar,
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
  },
  data() {
    return {
      searchModalShow: false,
      awaitingSearch: false,
      added_item_name: "",
      items: [],
      item_selected: null,
      //list users of selected group
      users: [],
      groupSearchQuery: null,
      userSearchQuery: null,
      userQuery: "",
      userselected: null,
      usersuggestions: [
        {
          data: [],
        },
      ],
      //table
      fields: [
        {
          key: "selected",
          label: "",
          thStyle: { width: "60px !important", minWidth: "60px !important" },
        },
        {
          key: "name",
          label: "Group name",
          sortable: true,
        },
        {
          key: "deleted",
          label: "",
          class: "text-center",
          thStyle: { width: "60px !important" },
        },
      ],
      fieldsUser: [
        {
          key: "account",
          label: "Username",
          sortable: true,
          thStyle: {
            width: "140px !important",
            minWidth: "140px !important",
            maxWidth: "140px !important",
          },
        },
        {
          key: "email",
          label: "Email",
          sortable: true,
          thStyle: { width: "300px !important" },
        },
        {
          key: "hovaten",
          label: "Full Name",
          sortable: true,
          thStyle: { width: "300px !important" },
        },
        {
          key: "deleted",
          label: "",
          class: "text-center",
          thStyle: { width: "60px !important" },
        },
      ],
    };
  },
  beforeRouteEnter(to, from, next) {
    axios.get("/api/role").then((response) => {
      next((vm) => vm.initData(response.data.items));
    });
  },
  beforeRouteUpdate(to, from, next) {
    axios
      .get("/api/role", { params: to.query })
      .then((role) => {
        this.items = role.data.items;
        this.$log(role.data);
        next();
      })
      .catch((err) => {
        next();
      });
  },
  methods: {
    initData(items) {
      this.items = items;
    },
    openSearchModal() {
      this.searchModalShow = true;
    },
    closeSearchModal() {
      this.searchModalShow = false;
    },
    getUserByRole() {
      if (this.item_selected == null) {
        this.user = [];
        return;
      }
      axios
        .get(`/api/role/${this.item_selected[0].id}/user`)
        .then((res) => {
          this.users = res.data.items;
        })
        .catch((err) => {
          this.users = [];
          this.$toast.error(err.response.data, "ERROR", { timeout: 3000 });
        });
    },
    findUserInRole(userSearchQuery) {
      if (this.item_selected == null) {
        this.user = [];
        return;
      }
      axios
        .get(
          `/api/role/${this.item_selected[0].id}/user?userSearchQuery=${userSearchQuery}`
        )
        .then((res) => {
          this.users = res.data.items;
        })
        .catch((err) => {
          this.users = [];
          this.$toast.error(err.response.data, "ERROR", { timeout: 3000 });
        });
    },
    onRowSelected(item) {
      if (item.length == 0) {
        this.item_selected = null;
        this.user = [];
      } else {
        this.item_selected = item;
        this.getUserByRole();
      }
    },
    onInputChange(text) {
      this.userselected = null;
      if (text == "") {
        this.usersuggestions = [
          {
            data: [],
          },
        ];
      } else {
        axios
          .get(`/api/user?q=${text}`)
          .then((res) => {
            this.usersuggestions = [
              {
                data: res.data,
              },
            ];
          })
          .catch((err) => {});
      }
    },
    onSelected(item) {
      this.userselected = item.item;
    },
    resetAutoSuggest() {
      this.userselected = null;
      this.usersuggestions = [
        {
          data: [],
        },
      ];
      this.userQuery = "";
    },
    getSuggestionValue(suggestion) {
      return suggestion.item.hovaten;
    },
    addItem() {
      if (!(this.added_item_name && this.added_item_name.length)) {
        this.$toast.error("Please enter group name!", "ERROR", {
          timeout: 2000,
        });
      } else {
        var isItemExist = this.items.filter((role) => role.name == this.added_item_name);
        if (isItemExist.length > 0)
          this.$toast.error("Group name already exists!", "ERROR", {
            timeout: 2000,
          });
        else {
          const CreatedBy = this.$store.state.auth.accountId;
          axios
            .post("/api/role", {
              name: this.added_item_name,
              CreatedBy: CreatedBy,
            })
            .then((res) => {
              this.items.push(res.data.item);
              this.added_item_name = "";
              this.$toast.success("Your Group has been added!", "SUCCESS", {
                timeout: 2000,
              });
            })
            .catch((err) => {
              this.$toast.error(err.response.data, "ERROR", { timeout: 3000 });
              this.added_item_name = "";
            });
        }
      }
    },
    deleteItem(data) {
      this.$toast.question("Do you really want to delete?", "WARNING", {
        timeout: 20000,
        close: false,
        overlay: true,
        displayMode: "once",
        position: "center",
        buttons: [
          [
            "<button><b>Yes</b></button>",
            (instance, toast) => {
              const updatedBy = this.$store.state.auth.accountId;
              axios
                .put(`api/role/${data.item.id}?UpdatedBy=${updatedBy}`)
                .then((res) => {
                  const rowPosition = this.items.indexOf(data.item);
                  this.items.splice(rowPosition, 1);
                  this.$toast.success("Your Group has been deleted!", "SUCCESS", {
                    timeout: 3000,
                  });
                })
                .catch((err) => {
                  this.$toast.error(err.response.data, "ERROR", {
                    timeout: 3000,
                  });
                });
              instance.hide({ transitionOut: "fadeOut" }, toast, "button");
            },
            true,
          ],
          [
            "<button>No</button>",
            function (instance, toast) {
              instance.hide({ transitionOut: "fadeOut" }, toast, "button");
            },
          ],
        ],
      });
    },
    updateItem(data, event) {
      var new_item_name = event.target.value;
      if (!(new_item_name && new_item_name.length)) {
        this.$toast.error("Please enter group name!", "ERROR", {
          timeout: 3000,
        });
        event.target.value = data.item.name;
      } else {
        var isItemExist = this.items.filter(
          (item) =>
            item.name.toLowerCase() === new_item_name.toLowerCase() &&
            item.id != data.item.id
        );
        if (isItemExist.length > 0) {
          this.$toast.error("Group name already exists!", "ERROR", {
            timeout: 3000,
          });
          event.target.value = data.item.name;
        } else {
          const updatedBy = this.$store.state.auth.accountId;
          axios
            .put("/api/role", {
              id: data.item.id,
              name: new_item_name,
              UpdatedBy: updatedBy,
            })
            .then((res) => {
              this.$toast.success("Your Group has been updated!", "SUCCESS", {
                timeout: 3000,
              });
              this.items[data.index].name = new_item_name;
            })
            .catch((err) => {
              this.$toast.error(err.response.data, "ERROR", { timeout: 3000 });
              event.target.value = data.item.name;
            });
        }
      }
    },
    addUser() {
      if (this.userselected == null) {
        this.$toast.error("Please select a user!", "ERROR", {
          timeout: 2000,
        });
      } else {
        var isItemExist = this.users.filter((item) => item.id == this.userselected.id);
        if (isItemExist.length > 0) {
          this.$toast.error("Your Account already exists!", "ERROR", {
            timeout: 3000,
          });
          this.resetAutoSuggest();
        } else {
          axios
            .post(`/api/role/${this.item_selected[0].id}/user`, {
              id: this.userselected.id,
            })
            .then((res) => {
              this.users.push(res.data.item);
              this.resetAutoSuggest();
              this.$toast.success("Your Account has been added!", "SUCCESS", {
                timeout: 3000,
              });
            })
            .catch((err) => {
              alert(err);
            });
        }
      }
    },

    deleteUser(data) {
      this.$toast.question("Do you really want to delete?", "WARNING", {
        timeout: 20000,
        close: false,
        overlay: true,
        displayMode: "once",
        position: "center",
        buttons: [
          [
            "<button><b>Yes</b></button>",
            (instance, toast) => {
              axios
                .delete(`/api/role/${this.item_selected[0].id}/user/${data.item.id}`)
                .then((res) => {
                  const rowPosition = this.users.indexOf(data.item);
                  this.users.splice(rowPosition, 1);
                  this.$toast.success("Your Account has been deleted!", "SUCCESS", {
                    timeout: 3000,
                  });
                })
                .catch((err) => {
                  this.$toast.error(err.response.data, "ERROR", {
                    timeout: 3000,
                  });
                });
              instance.hide({ transitionOut: "fadeOut" }, toast, "button");
            },
            true,
          ],
          [
            "<button>No</button>",
            function (instance, toast) {
              instance.hide({ transitionOut: "fadeOut" }, toast, "button");
            },
          ],
        ],
      });
    },
    groupSearchqueryItem() {
      let groupSearchQuery = Object.assign({}, this.$route.query);
      groupSearchQuery.q = this.groupSearchQuery;
      groupSearchQuery.page = 1;
      this.item = null;
      this.$router.push({ query: groupSearchQuery });
    },
    // userSearchQueryItem() {
    //   let userSearchQuery = Object.assign({}, this.$route.query);
    //   userSearchQuery.q = this.userSearchQuery;
    //   this.currentPage = 1;
    //   userSearchQuery.page = 1;
    //   this.$router.push({ query: userSearchQuery });
    // },
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style>
.demo #autosuggest {
  width: 600px;
  height: "30px !important";
  padding-top: 0;
  padding-bottom: 0;
  display: block;
}

.demo input {
  width: 100%;
}

.autosuggest__results-item--highlighted {
  background-color: rgba(217, 51, 51, 0.2);
  height: auto;
  padding-top: 0;
  padding-bottom: 0;
}
</style>
