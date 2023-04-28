<template>
  <div>
    <a-modal :visible="visible" title="Thêm quyền" @ok="addRole" @cancel="handleCancel">
      <a-form>
        <a-form-item label="Quyền" :rules="[{ required: true }]">
          <a-input v-model:value="form2.name" />
        </a-form-item>
        <!--         <a-form-item label="Description" :rules="[{ required: true }]">
            <a-input v-model:value="form2.description" />
          </a-form-item> -->
      </a-form>
    </a-modal>
    <a-button type="primary" @click="showModal" shape="round"><i class="fas fa-plus"/>Thêm quyền</a-button>
  </div>
</template>

<script>
import axios from "axios";
import { Modal, Form, Input, Select, message } from "ant-design-vue";

export default {
  components: {
    "a-modal": Modal,
    "a-form": Form,
    "a-form-item": Form.Item,
    "a-input": Input,
    "a-select": Select,
    "a-select-option": Select.Option,
    message
  },
  data() {
    return {
      visible: false,
      form2: {
        name: "",
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
    },
    addRole() {
      axios
        .post("/api/role", this.form2)
        .then((response) => {
          this.visible = false;
          this.$emit("addRole", response.data);
          message.success("Thêm quyền thành công");
          window.location.replace("/admin/roles");
        })
        .catch((error) => {
          console.log(error);
          message.error("Thêm thất bại");
        });
    },
  },
};
</script>
