<template>
  <div>
    <a-modal :visible="visible" title="Add Staff" @ok="addStaff" @cancel="handleCancel">
      <a-form :form="form">
        <a-form-item label="Name" :rules="[{ required: true }]">
          <a-input v-model="form.name" />
        </a-form-item>
        <a-form-item label="Username" :rules="[{ required: true }]">
          <a-input v-model="form.username" />
        </a-form-item>
        <a-form-item label="Email" :rules="[{ required: true }]">
          <a-input v-model="form.email" />
        </a-form-item>
        <a-form-item label="Phone" :rules="[{ required: true }]">
          <a-input v-model="form.phone" />
        </a-form-item>
        <a-form-item label="Status" :rules="[{ required: true }]">
          <a-select v-model="form.status">
            <a-select-option value="active">Active</a-select-option>
            <a-select-option value="inactive">Inactive</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </a-modal>
    <a-button type="primary" @click="showModal">Add Staff</a-button>
  </div>
</template>

<script>
import axios from "axios";
import { Modal, Form, Input, Select } from "ant-design-vue";

export default {
  components: {
    "a-modal": Modal,
    "a-form": Form,
    "a-form-item": Form.Item,
    "a-input": Input,
    "a-select": Select,
    "a-select-option": Select.Option,
  },
  data() {
    return {
      visible: false,
      form: {
        name: "",
        username: "",
        email: "",
        phone: "",
        status: "active",
      },
    };
  },
  methods: {
    showModal() {
      this.visible = true;
    },
    handleCancel() {
      this.visible = false;
      this.form.name = "";
      this.form.username = "";
      this.form.email = "";
      this.form.phone = "";
      this.form.status = "active";
    },
    addStaff() {
      axios
        .post("/api/staffs", this.form)
        .then((response) => {
          this.visible = false;
          this.$emit("addStaff", response.data);
          this.$message.success("Staff added successfully");
        })
        .catch((error) => {
          console.log(error);
          this.$message.error("Failed to add staff");
        });
    },
  },
};
</script>
