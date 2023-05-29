<template>
  <div>
    <a-card title="Khu vực">
      <div class="class= col-12 d-flex justify-content-end">
        <a-button type="primary" shape="round" @click="add">
          <i class="fas fa-plus"> </i>Thêm khu vực</a-button
        >
      </div>
      <a-table :columns="columns" :dataSource="areaDb">
        <template #bodyCell="{ column, index, text }">
          <template v-if="column.key === 'index'">
            <span>{{ index + 1 }}</span>
          </template>
          <template v-if="column.key === 'name'">{{ text }} </template>
          <template v-if="column.key === 'code'">{{ text }} </template>
          <template v-if:="column.dataIndex === 'description'">
            {{ text }}
          </template>
        </template>
        <template #customRender="{ record }">
          <a-space>
            <a-button type="primary" shape="round" @click="edit(record)">Sửa</a-button>
            <a-button type="danger" shape="round" @click="deleteArea(record.id)"
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
            label="Mã khu vực"
            :rules="[{ required: true, message: 'Vui lòng nhập mã khu vực' }]"
          >
            <a-input v-model:value="modalArea.code" />
          </a-form-item>
          <a-form-item
            label="Khu vực"
            :rules="[{ required: true, message: 'Vui lòng nhập tên khu vực' }]"
          >
            <a-input v-model:value="modalArea.name" />
          </a-form-item>

          <a-form-item
            label="Mô tả"
            :rules="[{ required: true, message: 'Vui lòng nhập mô tả khu vực' }]"
          >
            <a-input v-model:value="modalArea.description" />
          </a-form-item>
          <!--  <a-form-item>
              <a-button type="primary" htmlType="finish">Lưu</a-button>
              <a-button @click="cancel">Hủy</a-button>
            </a-form-item> -->
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
    useMenu().onSelectedKeys(["admin-areas"]);
  },
  setup() {
    const areaDb = ref([]);
    const modalArea = ref({});
    const showModal = ref(false);
    const modalTitle = ref("");
    const form = ref(null);

    const fetchAreaDb = async () => {
      const response = await axios.get("/api/area");
      areaDb.value = response.data;
    };

    const add = () => {
      modalArea.value = {};
      showModal.value = true;
      modalTitle.value = "Thêm khu vực";
      //form.value.resetFields();
    };

    const edit = (shiftType) => {
      modalArea.value = Object.assign({}, shiftType);
      showModal.value = true;
      modalTitle.value = "Sửa khu vực";
      //form.value.resetFields();
    };

    const save = async () => {
      const { id } = modalArea.value;
      const url = id ? `/api/area/${id}` : "/api/area";
      const method = id ? "put" : "post";
      const response = await axios[method](url, modalArea.value);
      //this.$toast.error(err.response.data, "Lỗi", { timeout: 3000 });
      fetchAreaDb();
      showModal.value = false;
    };
    /* DELETE MODAL */
    const deleteArea = (areaId) => {
      Modal.confirm({
        title: "Bạn có chắc muốn xóa khu vực này?",
        onOk: () => {
          axios
            .delete(`/api/area/${areaId}`)
            .then(() => {
              //area.splice(area.indexOf(area), 1);
              message.success("Khu vực đã được xóa ");
              window.location.replace("/admin/areas");
            })
            .catch((error) => {
              console.log(error);
              message.error("Xóa khu vực thất bại");
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
        title: "Mã khu vực",
        dataIndex: "code",
        key: "code",
      },
      {
        title: "Khu vực",
        dataIndex: "name",
        key: "name",
      },

      {
        title: "Mô tả",
        dataIndex: "description",
        key: "description",
        slots: { customRender: "description" },
      },
      {
        title: "Công cụ",
        key: "action",
        slots: { customRender: "customRender" },
      },
    ];

    fetchAreaDb();

    return {
      areaDb,
      modalArea,
      showModal,
      modalTitle,
      form,
      add,
      edit,
      save,
      deleteArea,
      cancel,
      columns,
    };
  },
};
</script>
