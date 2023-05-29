<template>
  <div>
    <a-card title="Quản lý nhân viên">
      <div class="class= col-12 d-flex justify-content-end">
        <a-button type="primary" shape="round" @click="add">
          <i class="fas fa-plus"> </i>Thêm nhân viên</a-button
        >
      </div>
      <a-table :columns="columns" :dataSource="StaffDb" :scroll="{ x: 1300, y: 1000 }">
        <template #bodyCell="{ column, index }">
          <template v-if="column.key === 'index'">
            <span>{{ index + 1 }}</span>
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
            label="Tài khoản"
            :rules="[{ required: true, message: 'Vui lòng nhập tài khoản' }]"
          >
            <a-input allow-clear v-model:value="modalStaff.username" />
          </a-form-item>
          <a-form-item
            label="Email"
            :rules="[{ required: true, message: 'Vui lòng nhập Email' }]"
          >
            <a-input allow-clear v-model:value="modalStaff.email" />
          </a-form-item>
          <a-form-item
            label="Tên nhân viên"
            :rules="[{ required: true, message: 'Vui lòng nhập tên' }]"
          >
            <a-input allow-clear v-model:value="modalStaff.name" />
          </a-form-item>
          <a-form-item
            label="Số điện thoại"
            :rules="[{ required: true, message: 'Vui lòng nhập số điện thoại' }]"
          >
            <a-input allow-clear v-model:value="modalStaff.phone" />
          </a-form-item>
          <a-form-item
            label="Phòng ban"
            :rules="[{ required: true, message: 'Vui lòng chọn khu vực' }]"
          >
            <a-select v-model:value="modalStaff.areaId">
              <a-select-option :key="area.id" :value="area.id" v-for="area in areas">
                {{ area.name }}
              </a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item
            label="Mật khẩu"
            :rules="[{ required: true, message: 'Vui lòng nhập mật khẩu' }]"
          >
            <a-input allow-clear v-model:value="modalStaff.password" />
          </a-form-item>
          <a-form-item
            label="Xác nhận mật khẩu"
            :rules="[{ required: true, message: 'Vui lòng nhập đúng mật khẩu' }]"
          >
            <a-input-password
              placeholder="Xác nhận mật khẩu"
              allow-clear
              v-model:value="modalStaff.repassword"
            />
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
import { Modal, message, Select } from "ant-design-vue";
export default {
  setup() {
    useMenu().onSelectedKeys(["admin-staff"]);
  },
  components: {
    "a-select": Select,
  },
  setup() {
    const StaffDb = ref([]);
    const modalStaff = ref([]);
    const showModal = ref(false);
    const modalTitle = ref("");
    const form = ref(null);
    const areas = ref([]);

    //Get the staff
    const fetchStaffDb = async () => {
      // Lấy dữ liệu từ API
      const response = await axios.get("/api/staff");
      StaffDb.value = response.data;
      //areas.value = response.data.areaName;
    };

    //Get the department
    const onMounted = async () => {
      // Lấy dữ liệu từ API
      const response = await axios.get("/api/area");
      areas.value = response.data;
    };
    const filterOption = (input, option) => {
      return option.label.toLowerCase().indexOf(input.toLowerCase()) >= 0;
    };
    /* CREATE MODAL */
    const add = () => {
      modalStaff.value = {};
      showModal.value = true;
      modalTitle.value = "Thêm nhân viên";
      //form.value.resetFields();
    };
    /* EDIT MODAL */
    const edit = (shiftType) => {
      modalStaff.value = Object.assign({}, shiftType);
      showModal.value = true;
      modalTitle.value = "Sửa nhân viên";
      //form.value.resetFields();
    };
    /* SELET MODAL EDIT OR CREATE */
    const save = async () => {
      // Lấy dữ liệu từ API
      const { id } = modalStaff.value;
      // Lấy dữ liệu từ API theo Id
      const url = id ? `/api/staff/${id}` : "/api/staff";
      const method = id ? "put" : "post";
      const response = await axios[method](url, modalStaff.value)
        .then(() => message(response))
        .catch(() => console.log(modalStaff));
      fetchStaffDb();
      showModal.value = false;
    };
    /* DELETE MODAL */
    const deleteArea = (StaffId) => {
      Modal.confirm({
        title: "Bạn có chắc muốn xóa khu vực này?",
        onOk: () => {
          axios
            .delete(`/api/staff/${StaffId}`)
            .then(() => {
              message.success("Nhân viên đã được xóa ");
              window.location.replace("/admin/staffs");
            })
            .catch((error) => {
              console.log(error);
              message.error("Xóa nhân viên thất bại");
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
        title: "Tài khoản",
        dataIndex: "username",
        key: "username",
      },
      {
        title: "Tên nhân viên",
        dataIndex: "name",
        key: "name",
      },
      {
        title: "Email",
        dataIndex: "email",
        key: "email",
        slots: { customRender: "email" },
      },
      {
        title: "Số điện thoại",
        dataIndex: "phone",
        key: "phone",
        slots: { customRender: "phone" },
      },
      {
        title: "Phòng ban",
        dataIndex: "areaName",
        slots: { customRender: "area" },
        key: "areaName",
        responsive: ["sm"],
      },
      {
        title: "Tình trạng",
        dataIndex: "status",
        key: "status",
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

    fetchStaffDb();
    onMounted();
    return {
      filterOption,
      areas,
      StaffDb,
      modalStaff,
      showModal,
      modalTitle,
      form,
      add,
      edit,
      save,
      deleteArea,
      cancel,
      columns,
      formatTime,
    };
  },
};
</script>
