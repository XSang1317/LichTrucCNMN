<template>
  <div>
    <a-card title="Danh sách khu vực">
      <div class="row mb-3">
        <div class="col-12 d-flex justify-content-end">
          <add-area></add-area>
        </div>
      </div>
      <a-table :columns="columns" :dataSource="area">
        <template #bodyCell="{ column, index, record }">
          <template v-if="column.key === 'index'">
            <span>{{ index + 1 }}</span>
          </template>

          <template v-else-if="column.key === 'actions'">
            <a-button type="submit" @click.prevent="showEditModal(record)">Edit</a-button>
            <a-button type="submit" @click.prevent="deleteArea(record.id)">Delete</a-button>
          </template>
        </template>
      </a-table>
    </a-card>
    <!-- Edit Modal -->
    <a-modal
      v-model="editModalVisible"
      title="Sửa khu vực"
      :ok-text="'Save'"
      :cancel-text="'Cancel'"
      @ok="saveArea"
      @cancel="cancelEditModal"
    >
      <a-form :form="editForm">
        <a-form-item
          label="Name"
          name="name"
          :rules="[{ required: true, message: 'Please enter name' }]"
        >
          <a-input v-model:value="editForm.name" />
        </a-form-item>
        <a-form-item label="Description" name="description">
          <a-input v-model:value="editForm.description" />
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script>
import { reactive, ref, onMounted } from "vue";
import { Modal, message } from "ant-design-vue";
import axios from "axios";
import addArea from "./addArea.vue";
export default {
  components: {
    "add-area": addArea,
    message,
  },

  setup() {
    const columns = [
      {
        title: "STT",
        key: "index",
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
      },
      {
        title: "Công cụ",
        key: "actions",
      },
    ];

    const area = reactive([]);
    onMounted(() => {
      axios
        .get("/api/area")
        .then((response) => {
          area.push(...response.data);
        })
        .catch((error) => {
          console.log(error);
          message.error("Failed to get area list");
        });
    });
    /* EDIT MODAL */
    const editingArea = ref(null);
    const editForm = reactive({
      name: "",
      description: "",
    });
    const editModalVisible = ref(false);

    const showEditModal = (record) => {
      editingArea.value = record;
      editForm.name = record.name;
      editForm.description = record.description;
      editModalVisible.value = true;
    };

    const saveArea = () => {
      axios.put(`/areas/${editingArea.value.id}`, editForm).then((res) => {
        getAreas();
        editModalVisible.value = false;
        editForm.name = "";
        editForm.description = "";
      });
    };

    const cancelEditModal = () => {
      editModalVisible.value = false;
      editForm.name = "";
      editForm.description = "";
    };
    /* DELTE MODAL */
    const deleteArea = (area, vueInstance) => {
      Modal.confirm({
        title: "Bạn có chắc muốn xóa khu vực này?",
        onOk: () => {
          axios
            .delete(`/api/area/${area}`)
            .then(() => {
              //area.splice(area.indexOf(area), 1);
              message.success("Khu vực đã bị xóa ");
              window.location.replace("/admin/areas");
            })
            .catch((error) => {
              console.log(error);
              message.error("Xóa khu vực thất bại");
            });
        },
      });
    };

    return {
      columns,
      area,
      addArea,
      editingArea,
      editForm,
      editModalVisible,
      showEditModal,
      saveArea,
      cancelEditModal,
      deleteArea,
    };
  },
};
</script>
