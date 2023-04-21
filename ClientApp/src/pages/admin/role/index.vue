<template>
  <a-card id="content-wrapper">
    <div class="container-fluid">
      <div class="row mb-3">
        <a-col :span="24">
          <div class="form-inline">
            <a-button
              type="primary"
              shape="round"
              :size="size"
              class="btn-sm border-0 btn-info mb-1 mr-2"
              style="float: right"
            >
              <router-link :to="{ name: 'admin-role-create' }">
                <i class="fa-solid fa-plus"></i>Thêm quyền
              </router-link>
            </a-button>
          </div>
        </a-col>
      </div>

      <div class="row">
        <div class="col-12">
          <a-table :dataSource="users" :columns="columns" :scroll="{ x: 576 }">
            <template #bodyCell="{ column, index }">
              <template v-if="column.key === 'index'">
                <span>{{ index + 1 }}</span>
              </template>

              <!--  <template v-if="column.key === 'status'">
              <span v-if="record.status_id == 1" class="text-primary">{{ record.status }}</span>
              <span v-else-if="record.status_id == 2" class="text-danger">{{ record.status }}</span>
            </template> -->
              <template v-else-if="column.dataIndex === 'action'">
                <a-popconfirm
                  v-if="users.length"
                  title="Sure to delete?"
                  @confirm="onDelete(record.key)"
                >
                  <a>Delete</a>
                </a-popconfirm>
              </template>
            </template>
          </a-table>
        </div>
      </div>
    </div>
  </a-card>
</template>
<script>
import { defineComponent, ref } from "vue";
import { useMenu } from "../../../stores/user-menu.js";
export default defineComponent({
  setup() {
    useMenu().onSelectedKeys(["admin-roles"]);

    const users = ref([]);
    const columns = [
      {
        title: "#",
        key: "index",
      },
      {
        title: "Tên quyền",
        dataIndex: "name",
        key: "name",
        responsive: ["sm"],
      },
      {
        title: "Công cụ",
        key: "action",
        fixed: "right",
      },
    ];
    /* const handleTableChange = (pag, filters, sorter) => {
      run({
        results: pag.pageSize,
        page: pag?.current,
        sortField: sorter.field,
        sortOrder: sorter.order,
        ...filters,
      });
    }; */
    const onDelete = (key) => {
      dataSource.value = dataSource.value.filter((item) => item.key !== key);
    };
    const getUsers = () => {
      axios
        .get("/api/role")
        .then((response) => {
          users.value = response.data;
        })
        .catch((error) => {
          console.log(error);
        });
    };

    getUsers();

    return {
      users,
      columns,
      onDelete,
    };
  },
});
</script>
