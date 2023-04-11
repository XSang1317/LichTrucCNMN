<template>
  <a-card>
    <div class="row mb-3">
      <div class="col-12 d-flex justify-content-end">
        <a-button type="primary">
          <router-link :to="{ name: 'admin-areas-create' }">
            <i class="fa-solid fa-plus"></i>
          </router-link>
        </a-button>
      </div>
    </div>

    <div class="row">
      <div class="col-12">
        <a-table
          :dataSource="users"
          :columns="columns"
          :scroll="{ x: 576 }"
        >
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
  </a-card>
</template>

<script>
import { defineComponent, ref } from "vue";
import { useMenu } from "../../../stores/user-menu.js";
export default defineComponent({
  setup() {
    useMenu().onSelectedKeys(["admin-areas"]);

    const users = ref([]);
    const columns = [
      {
        title: "#",
        key: "index",
      },
      {
        title: "Phòng ban",
        dataIndex: "name",
        key: "name",
        responsive: ["sm"],
      },
      {
        title: "Mô tả",
        dataIndex: "description",
        key: "description",
      },
     /*  {
        title: "Công cụ",
        key: "action",
        fixed: "right",
      }, */
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
    const getUsers = () => {
      axios
        .get("/api/area")
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
    };
  },
});
</script>
