<template>
  <div>
    <a-card title="Danh sách Quyền">
      <div class="row mb-3">
        <div class="col-12 d-flex justify-content-end">
          <add-role></add-role>
        </div>
      </div>
      <a-table :columns="columns" :dataSource="areaList">
        <template #bodyCell="{ column, index, record }">
          <template v-if="column.key === 'index'">
            <span>{{ index + 1 }}</span>
          </template>

          <template v-else-if="column.key === 'actions'">
            <a-button type="submit" @click.prevent="showEditModal()">Edit</a-button>
            <a-button type="submit" @click.prevent="deleteRole(record.id)">Delete</a-button>
          </template>
        </template>
      </a-table>
    </a-card>
  </div>
</template>

<script>
import { reactive, ref, onMounted } from "vue";
import {message, Modal}from 'ant-design-vue';
import axios from "axios";
import addRole from "./addRole.vue";
export default {
  components: {
    "add-role": addRole,
    Modal,
  },
  setup() {
    const columns = [
      {
        title: "STT",
        key: "index",
      },
      {
        title: "Quyền",
        dataIndex: "name",
        key: "name",
      },
      {
        title: "Công cụ",
        key: "actions",
      },
    ];

    const areaList = reactive([]);

    /* GET LIST MODAL */
    onMounted(() => {
      axios
        .get("/api/role")
        .then((response) => {
          areaList.push(...response.data);
        })
        .catch((error) => {
          console.log(error);
          message.error("Thất bại lấy danh sách Quyền");
        });
    });


    /* DELTE MODAL */
    const deleteRole = (role) => {
      Modal.confirm({
        title: "Bạn có chắc muốn xóa khu vực này?",
        onOk: () => {
          axios
            .delete(`/api/role/${role}`)
            .then(() => {
              //area.splice(area.indexOf(area), 1);
              message.success("Quyền đã được xóa ");
              window.location.replace("/admin/roles");
            })
            .catch((error) => {
              console.log(error);
              message.error("Xóa quyền thất bại");
            });
        },
      });
    };
    return {
      columns,
      areaList,
      addRole,
      deleteRole,
    };
  },
};
</script>
