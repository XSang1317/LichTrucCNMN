<template>
    <a-modal
      :visible="visible"
      title="Edit Area"
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
    <a-button type="primary" shape="round" @click="showModal">Add Area</a-button>
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
      form2: {
        name: "",
        description: " ",
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
      this.form.description = "";
    },
    addArea() {
      axios
        .post("/api/area", this.form2)
        .then((response) => {
          this.visible = false;
          this.$emit("addArea", response.data);
          this.$message.success("Area added successfully");
        })
        .catch((error) => {
          console.log(error);
          this.$message.error("Failed to add area");
        });
    },
  },
};
</script>
