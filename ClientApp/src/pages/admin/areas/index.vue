<template>
  <!-- SHOW MODAL -->
  <div id="content-wrapper">
    <div class="container-fluid">
      <a-row>
        <a-col :span="24">
          <div class="form-inline">
            <a-button
              type="primary"
              shape="round"
              :size="size"
              class="btn-sm border-0 btn-info mb-1 mr-2"
              style="float: right"
            >
              <router-link :to="{ name: 'admin-areas-create' }">
                <i class="fa-solid fa-plus"></i>Thêm khu vực
              </router-link>
            </a-button>
          </div>
        </a-col>
        <a-col :span="24">
          <div class="form-inline">
            <a-input
              class="mb-2 mr-2 h-auto pt-0 pb-0"
              style="flex-grow: 1; width: 1px"
              v-model="query"
              :placeholder="'Tìm theo tên và tên tắt'"
              @keydown.enter.prevent="queryItem"
            />
            <a-button
              class="btn-sm border-0 btn-outline-secondary mb-2"
              style="float: right"
              @click.prevent="queryItem"
            >
              <a-icon type="search" />
            </a-button>
          </div>
        </a-col>
      </a-row>
      <a-table
        :columns="columns"
        :dataSource="users"
        :rowKey="(data) => data.id"
        :items="areaType_Items"
        bordered
        size="small"
        :pagination="false"
        :scroll="{ x: 'max-content' }"
      >
        <template #bodyCell="{ column, index }">
          <template v-if="column.key === 'index'">
            <span>{{ index + 1 }}</span>
          </template>
        </template>
        <template v-slot:name="{ data }">
          {{ data.value }}
        </template>
        <template v-slot:action="{ data }">
          <a @click.prevent="openEditModal(data)">
            <a-icon type="edit" class="mr-1" />
          </a>
          <a @click.prevent="deleteItem(data)">
            <a-icon type="delete" class="text-danger" />
          </a>
        </template>
      </a-table>
    </div>
  </div>
  <!-- CREATED MODAL -->
  <a-modal
    v-model="createModalShow"
    :mask-closable="false"
    :keyboard="false"
    :width="520"
    :destroy-on-close="true"
  >
    <template #title> Thêm khu vực </template>
    <a-form :model="areaType" :rules="rules" @submit.prevent="submitCreate">
      <a-row>
        <a-col :span="24">
          <a-form-item label="Tên khu vực" required>
            <a-input v-model="areaType.name" />
          </a-form-item>
        </a-col>
      </a-row>
      <a-row>
        <a-col :span="24">
          <a-form-item label="Mô tả" required>
            <a-input v-model="areaType.description" />
          </a-form-item>
        </a-col>
      </a-row>
      <a-button
        type="primary"
        :loading="isLoading"
        :disabled="isDisabled"
        html-type="submit"
        >Lưu</a-button
      >
      <a-button @click="closeCreateModal">Đóng</a-button>
    </a-form>
  </a-modal>

  <!-- EDIT MODAL -->
  <a-modal
    :visible="editModalShow"
    :title="'Sữa Loại Hợp Đồng'"
    :mask-closable="false"
    :keyboard="false"
    :footer="null"
  >
    <a-form @keydown.enter.prevent="submitEdit()" @submit.prevent="submitEdit()">
      <a-row>
        <a-col>
          <a-form-item label="Tên Loại Hợp Đồng" :colon="false" :required="true">
            <a-input
              v-model="areaType.name"
              class="h-auto pt-0 pb-0"
              :placeholder="'Vui lòng nhập tên loại Hợp đồng !'"
            />
          </a-form-item>
        </a-col>
      </a-row>
      <a-row>
        <a-col>
          <a-form-item label="Tên viết tắt" :colon="false" :required="true">
            <a-input
              v-model="areaType.initials"
              class="h-auto pt-0 pb-0"
              :placeholder="'Vui lòng nhập Tên viết tắt'"
            />
          </a-form-item>
        </a-col>
      </a-row>

      <a-button
        class="btn-sm border-0 btn-secondary float-right"
        @click.prevent="closeEditModal"
      >
        <a-icon type="close" class="mr-2" /> Close
      </a-button>
      <a-button class="btn-sm border-0 btn-info mr-3 float-right" :disabled="isDisabled">
        <a-icon type="save" class="mr-2" /> Save
      </a-button>
    </a-form>
  </a-modal>
</template>

<script>
import axios from "axios";
import { defineComponent, ref } from "vue";
import { useMenu } from "../../../stores/user-menu.js";
export default defineComponent({
  computed: {
    crud_category() {
      return this, $store.getters.hasPermission("crud_category");
    },
  },
  beforeRouteEnter(to, from, next) {
    axios.get("/api/area/").then((areaType) => {
      next((vm) => vm.initData(areaType.data));
    });
  },
  beforeRouteUpdate(to, from, next) {
    axios
      .get("/api/area/GetAllarea", { params: to.query })
      .then((areaType) => {
        this.areaType_Items = areaType.data;
        next();
      })
      .catch((err) => {
        next();
      });
  },
  methods: {
    initData(areaType) {
      this.areaType_Items = areaType;
    },

    /* Created Method */
    openCreateModal() {
      this.areaType.id = 0;
      this.areaType.name = "";
      this.areaType.description = "";
      this.createModalShow = true;
    },
    closeCreateModal() {
      this.createModalShow = false;
    },
    submitCreate() {
      this.isDisabled = true;
      this.areaType.createdBy = this.$store.state.auth.usernameId;
      axios.post("/api/area/create");
    },
  },
  setup() {
    useMenu().onSelectedKeys(["admin-areas"]);

    const users = ref([]);
    const columns = [
      {
        title: "#",
        key: "index",
      },
      {
        title: "Phòng ban",
        dataIndex: "name",
        key: "name",
        responsive: ["sm"],
        style: "text-align:center",
        sorter: true,
      },
      {
        title: "Mô tả",
        dataIndex: "description",
        key: "description",
        sorter: true,
      },
      {
        title: "",
        key: "action",
        fixed: "right",
      },
    ];

    const getUsers = () => {
      axios
        .get("/api/area")
        .then((response) => {
          users.value = response.data;
        })
        .catch((error) => {
          console.log(error);
        });
    };
    getUsers();
    return {
      users,
      columns,
    };
  },
});
</script>
