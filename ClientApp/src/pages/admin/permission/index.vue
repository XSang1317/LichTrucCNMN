<template>
  <div id="wrapper">
    <div id="content-wrapper">
      <div class="content-fluid">
        <div class="row">
          <div class="col-3">
            <a-table
              :columns="columns"
              :dataSource="fields"
              selectable
              rowKey="id"
              :items="items"
              v-on:row-selected="onRowSelected"
              class="table table-striped table-hover table-bordered"
            >
              <template  v-slot:cell(selected)="{ rowSelected }">
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
              :columns="permissionGroupsColumns"
              :dataSource="permissionGroups"
              rowKey="id"
              v-on:row-selected="onPermissionGroupRowSelected"
              class="table table-striped table-hover table-bordered"
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
              :columns="activityColumns"
              :dataSource="permissionsByGroup"
              rowKey="id"
              class="table table-striped table-hover table-bordered"
            >
              <template v-slot:cell(hasPermission)="data">
                <div class="text-center">
                  <a-checkbox
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
         
        },
        {
          key: "name",
          label: "Group",
          dataIndex: "name",
          
          sortable: true,
        },
      ],
      //table permission group
      fieldsPermissionGroups: [
        {
          key: "selected",
          label: "",
         
        },
        {
          key: "name",
          label: "Role",
        
          sortable: true,
        },
      ],
      //table permission
      fieldsActivity: [
        {
          key: "hasPermission",
          label: "",
          
        },
        {
          key: "name",
          label: "Permission",
          
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
      axios
        .put(`api/role/${this.item_selected[0].id}/permissions`, {
          id: data.item.id,
          hasPermission: data.ite.hasPermission,
        })
        .then((res) => {})
        .catch((err) => {
          this.$toast.error(err.response.data, "Error", { timeout: 3000 });
        });
    },
  },
};
</script>
<style scoped></style>
