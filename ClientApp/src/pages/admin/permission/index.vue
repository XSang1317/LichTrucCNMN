<template>
  <div class="content-wrapper">
    <a-row :gutter="16">
      <a-col :span="8">
        <a-table
          :columns="columns"
          :data-source="items"
          :row-key="(record) => record.id"
          :row-selection="{ type: 'radio', onRowSelected }"
          :loading="isLoading"
        />
      </a-col>
      <a-col :span="8">
        <a-table
          :columns="permissionGroupColumns"
          :data-source="permission_groups"
          :row-key="(record) => record.id"
          :row-selection="{
            type: 'radio',
            onPermissionGroupRowSelected,
          }"
          :loading="isLoading"
        />
      </a-col>
      <a-col :span="8">
        <a-table
          :columns="permissionColumns"
          :data-source="permissionByGroup"
          :row-key="(record) => record.id"
          :loading="isLoading"
        >
          <template #hasPermission="{ record }">
            <a-checkbox
              v-model="record.hasPermission"
              @change="updatePermissionInRole(record)"
            />
          </template>
        </a-table>
      </a-col>
    </a-row>
  </div>
</template>
<script>
import axios from "axios";
import { Table, Checkbox } from "ant-design-vue";
export default {
  name: "Permissions",

  components: { "a-table": Table, "a-checkbox": Checkbox },
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
        /*  {
          title: "Selected",
          dataIndex: "selected",
          scopedSlots: { customRender: "selected" },
          key: "selected",
          width: 100,
        }, */
        { key: "name", title: "Group", dataIndex: "name" },
      ],
      permissionGroupColumns: [{ key: "name", title: "Role", dataIndex: "name" }],
      permissionColumns: [
        { key: "hasPermission", dataIndex: "hasPermission", title: "" },
        { key: "name", title: "Permission", dataIndex: "name", sortable: true },
      ],
    };
  },
  beforeRouteEnter(to, from, next) {
    axios.all([axios.get("/api/role"), axios.get("/api/permissiongroup")]).then(
      axios.spread((role, permission_groups) => {
        next((vm) => vm.initData(role.data, permission_groups.data.items));
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
        .get(`/api/role/${this.item_selected.id}/permissiongroup`)
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
