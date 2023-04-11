<template>
  <div id="wrapper">
    <div id="content-wrapper">
      <div class="content-fluid">
        <div class="row">
          <div class="col-3">
            <a-table
              borderless
              small
              striped
              bordered
              :fields="fields"
              selectable
              select-mode="single"
              responsive
              :items="items"
              @row-selected="onRowSelected"
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
            </a-table>
          </div>
          <div class="col-5">
            <a-table
              bordered
              striped
              small
              thead-class="text-center"
              selectable
              select-mode="single"
              hover
              responsive
              :fields="fieldsPermissionGroups"
              :items="permission_groups"
              @row-selected="onPermissionGroupRowSelected"
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
            </a-table>
          </div>
          <div class="col-4">
            <a-table
              thead-class="text-center"
              bordered
              striped
              small
              head-variant="dark"
              hover
              responsive
              :fields="fieldsActivity"
              :items="permissionsByGroup"
              table-variant="light"
            >
              <template v-slot:cell(hasPermission)="data">
                <div class="text-center">
                  <input
                    type="checkbox"
                    v-model="data.item.hasPermission"
                    @change.prevent="updatePermissionInRole(data)"
                  />
                </div>
              </template>
            </a-table>
          </div>
        </div>
      </div>
      <!-- /.container-fluid -->
    </div>
  </div>
</template>
<script>
import axios from "axios";
export default {
  name: "Permissions",
  components: {},
  computed: {
    hethong_update() {
      return this.$store.getters.hasPermission("hethong_update");
    },
    permissionByGroup() {
      if (this.item_selected != null && this.permission_group_selected != null) {
        let result = [];
        const vm = this;
        this.permissions.forEach(function (permission) {
          if (permission.permissionGroupId == vm.permission_groups_selected[0].id) {
            result.push(permission);
          }
        });
        return result;
      }
      return [];
    },
  },
  data() {
    return {
      items: [],
      item_selected: null,
      permission: [],
      permission_groups: [],
      permission_group_selected: null,
      //Column Table
      fields: [
        {
          key: "selected",
          label: "",
          thStyle: { width: "30px !important", minWidth: "30px !important" },
        },
        {
          key: "name",
          label: "Group",
          thStyle: { width: "300px !important", minWidth: "130px !important" },
          sortable: true,
        },
      ],
      //table permission group
      fieldsPermissionGroups: [
        {
          key: "selected",
          label: "",
          thStyle: { width: "30px !important", minWidth: "30px !important" },
        },
        {
          key: "name",
          label: "Role",
          thStyle: { width: "500px !important", minWidth: "160px !important" },
          sortable: true,
        },
      ],
      //table permission
      fieldsActivity: [
        {
          key: "hasPermission",
          label: "",
          thStyle: { width: "60px !important", minWidth: "60px !important" },
        },
        {
          key: "name",
          label: "Permission",
          thStyle: { width: "400px !important", minWidth: "160px !important" },
          sortable: true,
        },
      ],
    };
  },
  beforeRouteEnter(to, from, next) {
    axios.all([axios.get("/api/role"), axios.get("/api/permissiongroups")]).then(
      axios.spread((role, permission_groups) => {
        next((vm) => vm.initData(role.data.items, permission_groups.data.items));
      })
    );
  },
  methods: {
    initData(items, permission_groups) {
      this.items = items;
      this.permission_groups = permission_groups;
    },
    onRowSelected(item) {
      if (item.length == 0) {
        this.item_selected = null;
      } else {
        this.item_selected = item;
        this.getPermissionsByRole();
      }
    },
    onPermissionGroupRowSelected(item) {
      if (item.length == 0) {
        this.permission_group_selected = null;
      } else {
        this.permission_group_selected = item;
      }
    },
    getPermissionsByRole(item) {
      if (item.lenght == null) {
        this.permission = [];
        return;
      }
      axios
        .get(`/api/role/${this.item_selected[0].id}/permissions`)
        .then((res) => {
          this.permission = res.data.items;
        })
        .catch((err) => {
          this.permission = [];
          this.$toast.error(err.response.data, "Error", { timeout: 3000 });
        });
    },
    updatePermissionInRole(data) {
      if (this.item_selected == null) {
        return;
      }
      axios.put(`api/role/${this.item_selected[0].id}/permissions`, {
        id: data.item.id,
        hasPermission: data.ite.hasPermission,
      }).then((res)=> {})
      .catch((err)=> {
        this.$toast.error(err.response.data,"Error", {timeout : 3000});
      });
    },
  },
};
</script>
<style scoped></style>