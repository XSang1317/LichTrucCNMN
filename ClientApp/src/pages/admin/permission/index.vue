<template>
  <div id="wrapper">
    <div class="content-wrapper">
      <div class="container-fluid">
        <div class="row">
          <div class="col-3">
            <a-table
              :columns="columns"
              :data-source="items"
              :row-key="'id'"
              row-selection="{type: 'radio', onChange: onRowSelected}"
            >
              <template #selected="{ selected }">
                <div class="text-center">
                  <template v-if="selected">
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
              :columns="permissionGroupColumns"
              :data-source="permission_groups"
              :row-key="'id'"
              row-selection="{type: 'radio', onChange: onPermissionGroupRowSelected}"
            >
              <template #selected="{ selected }">
                <div class="text-center">
                  <template v-if="selected">
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
              :columns="permissionColumns"
              :data-source="permissionsByGroup"
              :row-key="'id'"
            >
              <template #hasPermission="{ record }">
                <div class="text-center">
                  <a-checkbox
                    v-model="record.hasPermission"
                    @change="updatePermissionInRole(record)"
                  />
                </div>
              </template>
            </a-table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import axios from "axios";
import { Table, Checkbox } from 'ant-design-vue';
export default {
  name: "Permissions",
  components: {
    'a-table': Table,
    'a-checkbox': Checkbox,
  },
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
      permissions: [],
      permission_groups: [],
      permission_groups_selected: null,
      columns: [
        {
          title: 'Selected',
          dataIndex: 'selected',
          scopedSlots: { customRender: 'selected' },
          width: 100,
        },
        {
          key: "name",
          title: "Group",
          dataIndex: "name",
        },
        // add other columns as needed
      ],
      permissionGroupColumns: [
        {
          title: 'Selected',
          dataIndex: 'selected',
          width: 100,
          scopedSlots: { customRender: 'selected' },
          
        },
        {
          key: "name",
          title: "Role",
          dataIndex:"name",
        },
        // add other columns as needed
      ],
      permissionColumns: [
        {
          key: "hasPermission",
          dataIndex: "hasPermission",
          title: "",
        },
        {
          key: "name",
          title: "Permission",
          dataIndex: "name",
          sortable: true,
        },
      ],
    };
  },
/*   data() {
    return {
      items: [],
      item_selected: null,
      permission: [],
      permissionsByGroup: [],
      permission_group_selected: null,
      //Column Table
      columns: [
        {
          key: "selected",
          label: "",
        },
        {
          key: "name",
          label: "Group",
          dataIndex: "name",
        },
      ],
      //table permission group
      permissionGroupsColumns: [
        {
          key: "selected",
          title: "",
        },
        {
          key: "name",
          title: "Role",
        },
      ],
      //table permission
      activityColumns: [
        {
          key: "hasPermission",
          dataIndex: "hasPermission",
          title: "",
        },
        {
          key: "name",
          title: "Permission",
          dataIndex: "name",
          sortable: true,
        },
      ],
    };
  }, */
  beforeRouteEnter(to, from, next) {
    axios.all([axios.get("/api/role"), axios.get("/api/permissiongroup")]).then(
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
        .get(`/api/role/${this.item_selected[0].id}/permissiongroup`)
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
        .put(`api/role/${this.item_selected[0].id}/permissiongroup`, {
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
