<template>
    <a-modal :visible="visible" title="Thêm khu vực" @ok="addArea" @cancel="handleCancel">
      <a-form>
        <a-form-item label="Tên khu vực" :rules="[{ required: true }]">
       <a-input v-model:value="form2.name" />
        </a-form-item>
        <a-form-item label="Mô tả" :rules="[{ required: true }]">
          <a-input v-model:value="form2.description" />
        </a-form-item>
      </a-form>
    </a-modal>
    <a-button type="primary" @click="showModal" shape="round"><i class="fas fa-plus"/>Thêm khu vực</a-button>
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
          this.$message.success("Thêm khu vực thành công");
          //this.$router.push({name: "admin-areas"});
          window.location.replace("/admin/areas");
        })
        .catch((error) => {
          console.log(error);
          this.$message.error("Thêm khu vực thất bại");
        });
    },
  },
};
</script>
