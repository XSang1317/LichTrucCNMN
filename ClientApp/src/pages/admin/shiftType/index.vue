<template>
  <div>
    <a-card title="Shift Types">
      <a-table :columns="columns" :dataSource="shiftTypes">
        <template #timeStart="{ text }">
          {{ text | formatTime }}
        </template>
        <template #timeEnd="{ text }">
          {{ text | formatTime }}
        </template>
        <template #customRender="{ record }">
          <a-space>
            <a-button type="primary" @click="edit(record)">Sửa</a-button>
            <a-button type="danger" @click="deleteShiftType(record)">Xóa</a-button>
          </a-space>
        </template>
      </a-table>
      <a-button type="primary" @click="add">Thêm loại ca</a-button>
      <a-modal v-model:visible="showModal" :title="modalTitle" @cancel="cancel">
        <a-form :form="form" @finish="save">
          <a-form-item
            label="Name"
            :rules="[{ required: true, message: 'Please input name' }]"
          >
            <a-input v-model:value="modalShiftType.name" />
          </a-form-item>
          <a-form-item
            label="Time Start"
            :rules="[{ required: true, message: 'Please input time start' }]"
          >
            <a-time-picker v-model:value="modalShiftType.timeStart" />
          </a-form-item>
          <a-form-item
            label="Time End"
            :rules="[{ required: true, message: 'Please input time end' }]"
          >
            <a-time-picker v-model:value="modalShiftType.timeEnd" />
          </a-form-item>
          <a-form-item>
            <a-button type="primary" htmlType="submit">Lưu</a-button>
            <a-button @click="cancel">Hủy</a-button>
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
export default {
  setup() {
    useMenu().onSelectedKeys(["admin-type"]);
  },
  setup() {
    const shiftTypes = ref([]);
    const modalShiftType = ref({});
    const showModal = ref(false);
    const modalTitle = ref("");
    const form = ref(null);

    const fetchShiftTypes = async () => {
      const response = await axios.get("/api/shiftstype");
      shiftTypes.value = response.data;
    };

    const add = () => {
      modalShiftType.value = {};
      showModal.value = true;
      modalTitle.value = "Thêm Loại Ca";
      form.value.resetFields();
    };

    const edit = (shiftType) => {
      modalShiftType.value = { ...shiftType };
      showModal.value = true;
      modalTitle.value = "Sửa loại ca";
      form.value.resetFields();
    };

    const save = async () => {
      const { id } = modalShiftType.value;
      const url = id ? `/api/shiftstype/${id}` : "/api/shiftstype";
      const method = id ? "put" : "post";
      const response = await axios[method](url, modalShiftType.value);
      fetchShiftTypes();
      showModal.value = false;
    };

    const deleteShiftType = async (shiftType) => {
      const { id } = shiftType;
      await axios.delete(`/api/shiftstype/${id}`);
      fetchShiftTypes();
    };

    const cancel = () => {
      showModal.value = false;
    };

    const columns = [
      {
        title: "STT",
        key: "index",
      },
      {
        title: "Name",
        dataIndex: "name",
        key: "name",
      },
      {
        title: "Time Start",
        dataIndex: "timeStart",
        key: "timeStart",
        slots: { customRender: "timeStart" },
      },
      {
        title: "Time End",
        dataIndex: "timeEnd",
        key: "timeEnd",
        slots: { customRender: "timeEnd" },
      },
      {
        title: "Action",
        key: "action",
        slots: { customRender: "customRender" },
      },
    ];

    const formatTime = (time) => {
      return moment(time).format("h:mm A");
    };

    fetchShiftTypes();

    return {
      shiftTypes,
      modalShiftType,
      showModal,
      modalTitle,
      form,
      add,
      edit,
      save,
      deleteShiftType,
      cancel,
      columns,
      formatTime,
    };
  },
};
</script>
