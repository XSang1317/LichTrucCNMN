<template>
  <div>
    <a-card title="Danh sách khu vực">
      <div class="row mb-3">
        <div class="col-12 d-flex justify-content-end">
          <a-button
            type="primary"
            shape="round"
            class="btn-sm border-0 btn-info mb-1 mr-2"
            style="float: left"
          >
            <router-link :to="{ name: 'admin-areas-create' }">
              <i class="fa-solid fa-plus"></i>Thêm khu vực
            </router-link>
          </a-button>
          <!-- <add-area @addArea="handleAddArea" /> -->
        </div>
      </div>
      <a-table :columns="columns" :dataSource="areaList">
        <template #bodyCell="{ column, index, record }">
          <template v-if="column.key === 'index'">
            <span>{{ index + 1 }}</span>
          </template>

          <template v-else-if="column.dataIndex === 'action'">
            <a @click="showEditModal(record)">Edit</a>
            <a-divider type="vertical" />
            <a @click="deleteArea(record)">Delete</a>
          </template>
        </template>
      </a-table>
    </a-card>
    <!-- Created Modal -->
    <a-modal
      v-model:visible="addModalVisible"
      title="Add New Area"
      :ok-text="'Add'"
      :cancel-text="'Cancel'"
      @ok="addArea"
      @cancel="cancelAddModal"
    >
      <a-form :form="addForm">
        <a-form-item
          label="Name"
          name="name"
          :rules="[{ required: true, message: 'Please enter name' }]"
        >
          <a-input v-model:value="addForm.name" />
        </a-form-item>
        <a-form-item label="Description" name="description">
          <a-input v-model:value="addForm.description" />
        </a-form-item>
      </a-form>
    </a-modal>
    <!-- Edit Modal -->
    <a-modal
      v-model="editModalVisible"
      title="Edit Area"
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
//import { message, Modal, Form, Input, Button, Table, Divider } from "ant-design-vue";
import axios from "axios";
import addArea from "./addArea.vue";
export default {
  components: {
    "add-area": addArea,
  },
  
  setup() {
    const columns = [
      {
        title: "Name",
        dataIndex: "name",
        key: "name",
      },
      {
        title: "Description",
        dataIndex: "description",
        key: "description",
      },
      {
        title: "Actions",
        key: "actions",
      },
    ];

    const areaList = reactive([]);

    const addModalVisible = ref(false);
    const addForm = ref(null);

    const cancelAddModal = () => {
      addModalVisible.value = false;
      addForm.value.resetFields();
    };

    const showAddModal = () => {
      addModalVisible.value = true;
    };

    const addArea = () => {
      addForm.value.validateFields().then((values) => {
        const newArea = {
          name: values.name,
          description: values.description,
        };
        axios
          .post("/api/area", newArea)
          .then((response) => {
            areaList.push(response.data);
            message.success("Area added successfully");
            addModalVisible.value = false;
            addForm.value.resetFields();
          })
          .catch((error) => {
            console.log(error);
            message.error("Failed to add area");
          });
      });
    };

    const editModalVisible = ref(false);
    const editForm = ref(null);
    const editingArea = ref(null);

    const cancelEditModal = () => {
      editModalVisible.value = false;
      editForm.value.resetFields();
    };

    const showEditModal = (area) => {
      editingArea.value = area;
      editForm.value.setFieldsValue({
        name: area.name,
        description: area.description,
      });
      editModalVisible.value = true;
    };

    const saveArea = () => {
      editForm.value.validateFields().then((values) => {
        const updatedArea = {
          ...editingArea.value,
          name: values.name,
          description: values.description,
        };
        axios
          .put(`/api/area/${updatedArea.id}`, updatedArea)
          .then(() => {
            areaList.splice(areaList.indexOf(editingArea.value), 1, updatedArea);
            message.success("Area updated successfully");
            editModalVisible.value = false;
            editForm.value.resetFields();
          })
          .catch((error) => {
            console.log(error);
            message.error("Failed to update area");
          });
      });
    };

    const deleteArea = (area) => {
      Modal.confirm({
        title: "Are you sure to delete this area?",
        onOk: () => {
          axios
            .delete(`/api/area/${area.id}`)
            .then(() => {
              areaList.splice(areaList.indexOf(area), 1);
              message.success("Area deleted successfully");
            })
            .catch((error) => {
              console.log(error);
              message.error("Failed to delete area");
            });
        },
      });
    };

    onMounted(() => {
      axios
        .get("/api/area")
        .then((response) => {
          areaList.push(...response.data);
        })
        .catch((error) => {
          console.log(error);
          message.error("Failed to get area list");
        });
    });

    return {
      columns,
      areaList,
      addModalVisible,
      addForm,
      cancelAddModal,
      showAddModal,
      addArea,
      editModalVisible,
      editForm,
      editingArea,
      cancelEditModal,
      showEditModal,
      saveArea,
      deleteArea,
    };
  },
};
</script>
