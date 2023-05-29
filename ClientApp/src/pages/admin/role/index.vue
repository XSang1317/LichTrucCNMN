<template>
  <div>
    <a-card title="Quyền">
      <div class="class= col-12 d-flex justify-content-end">
        <a-button type="primary" shape="round" @click="add">
          <i class="fas fa-plus"> </i>Thêm Quyền</a-button
        >
      </div>
      <a-table :columns="columns" :dataSource="roleDb">
        <template #bodyCell="{ column, index, text }">
          <template v-if="column.key === 'index'">
            <span >{{ index + 1 }}</span>
          </template>
          <template v-if="column.key === 'name'">{{ text }} </template>
        </template>
        <template #customRender="{ record }">
          <a-space>
            <a-button type="primary" shape="round" @click="edit(record)">Sửa</a-button>
            <a-button type="danger" shape="round" @click="deleterole(record.id)"
              >Xóa</a-button
            >
          </a-space>
        </template>
      </a-table>

      <a-modal
        v-model:visible="showModal"
        :title="modalTitle"
        @ok="save"
        @cancel="cancel"
      >
        <a-form :form="form" @finish="save">
          <a-form-item
            label="Quyền"
            :rules="[{ required: true, message: 'Vui lòng nhập tên Quyền' }]"
          >
            <a-input v-model:value="modalrole.name" />
          </a-form-item>
        </a-form>
      </a-modal>
    </a-card>
  </div>
</template>

<script>
import { ref } from "vue";
import axios from "axios";
import { useMenu } from "../../../stores/user-menu.js";
import { Modal, message } from "ant-design-vue";
export default {
  setup() {
    useMenu().onSelectedKeys(["admin-roles"]);
  },
  setup() {
    const roleDb = ref([]);
    const modalrole = ref({});
    const showModal = ref(false);
    const modalTitle = ref("");
    const form = ref(null);

    const fetchroleDb = async () => {
      const response = await axios.get("/api/role");
      roleDb.value = response.data;
    };

    const add = () => {
      modalrole.value = {};
      showModal.value = true;
      modalTitle.value = "Thêm Quyền";
      //form.value.resetFields();
    };

    const edit = (shiftType) => {
      modalrole.value = Object.assign({}, shiftType);
      showModal.value = true;
      modalTitle.value = "Sửa Quyền";
      //form.value.resetFields();
    };

    const save = async () => {
      const { id } = modalrole.value;
      const url = id ? `/api/role/${id}` : "/api/role";
      const method = id ? "put" : "post";
      const response = await axios[method](url, modalrole.value);
      //this.$toast.error(err.response.data, "Lỗi", { timeout: 3000 });
      fetchroleDb();
      showModal.value = false;
    };
    /* DELETE MODAL */
    const deleterole = (roleId) => {
      Modal.confirm({
        title: "Bạn có chắc muốn xóa Quyền này?",
        onOk: () => {
          axios
            .delete(`/api/role/${roleId}`)
            .then(() => {
              //role.splice(role.indexOf(role), 1);
              message.success("Quyền đã được xóa ");
              window.location.replace("/admin/roles");
            })
            .catch((error) => {
              console.log(error);
              message.error("Xóa Quyền thất bại");
            });
        },
      });
    };

    const cancel = () => {
      showModal.value = false;
    };

    const columns = [
      {
        title: "STT",
        key: "index",
        align:"center"
      },
      {
        title: "Quyền",
        dataIndex: "name",
        key: "name",
      },
      {
        title: "Công cụ",
        key: "action",
        slots: { customRender: "customRender" },
      },
    ];

    fetchroleDb();

    return {
      roleDb,
      modalrole,
      showModal,
      modalTitle,
      form,
      add,
      edit,
      save,
      deleterole,
      cancel,
      columns,
    };
  },
};
</script>
