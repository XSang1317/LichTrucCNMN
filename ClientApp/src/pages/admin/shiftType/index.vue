<template>
  <div>
    <a-card title="Loại ca">
      <div class="class= col-12 d-flex justify-content-end">
        <a-button type="primary" shape="round" @click="add">
          <i class="fas fa-plus"> </i>Thêm loại ca</a-button
        >
      </div>
      <a-table :columns="columns" :dataSource="shiftTypes">
        <template #bodyCell="{ column, index, text }">
          <template v-if="column.key === 'index'">
            <span>{{ index + 1 }}</span>
          </template>
          <template v-if="column.key === 'name'">{{ text }} </template>
          <template v-if:="column.dataIndex === 'timestar'">
            {{ text | formatTime }}
          </template>
          <template v-if="column.dataIndex === 'timeend'">
            {{ text | formatTime }}
          </template>
        </template>
        <template #customRender="{ record }">
          <a-space>
            <a-button type="primary" shape="round" @click="edit(record)">Sửa</a-button>
            <a-button type="danger" shape="round" @click="deleteShiftType(record.id)"
              >Xóa</a-button
            >
          </a-space>
        </template>
      </a-table>

      <a-modal
        v-model:visible="showModal"
        :title="modalTitle"
        @ok = "save"
        @cancel="cancel"
      >
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
      form.value.resetFields(record);
    };


    const edit = (shiftType) => {
      modalShiftType.value = Object.assign({}, shiftType);
      showModal.value = true;
      modalTitle.value = "Sửa loại ca";
      form.value.resetFields(record);
    };
    
    const save = async () => {
      const { id } = modalShiftType.value;
      const url = id ? `/api/shiftstype/${id}` : "/api/shiftstype";
      const method = id ? "put" : "post";
      const response = await axios[method](url, modalShiftType.value);
      fetchShiftTypes();
      showModal.value = false;
    };
        /* DELETE MODAL */
    const deleteShiftType = (shiftstype) => {
      Modal.confirm({
        title: "Bạn có chắc muốn xóa ca này?",
        onOk: () => {
          axios
            .delete(`/api/shiftstype/${shiftstype}`)
            .then(() => {
              //area.splice(area.indexOf(area), 1);
              message.success("Ca đã được xóa ");
              window.location.replace("/admin/type");
            })
            .catch((error) => {
              console.log(error);
              message.error("Xóa ca thất bại");
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
        title: "Loại ca",
        dataIndex: "name",
        key: "name",
      },
      {
        title: "Thời gian bắt đầu",
        dataIndex: "timestart",
        key: "timestart",
        slots: { customRender: "timeStart" },
      },
      {
        title: "Thời gian kết thúc",
        dataIndex: "timeend",
        key: "timeend",
        slots: { customRender: "timeEnd" },
      },
      {
        title: "Công cụ",
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
