<template>
  <form @submit.prevent="createArea()">
    <a-card title="Tạo khu vực mới" style="width: 100%">
      <div class="row mb-3">
        <!-- Image -->
        <div class="col-8 col-sm-12">
          <!-- <div class="row mb-3">
          <div class="col-12 col-sm-3 text-start text-sm-end">
            <label>
              <span class="text-danger me-1">*</span>
              <span>Tên Tài khoản:</span>
            </label>
          </div>

          <div class="col-12 col-sm-5">
            <a-input placeholder="Tên Tài khoản" allow-clear />
          </div>
        </div> -->

          <div class="row mb-3">
            <div class="col-12 col-sm-3 text-start text-sm-end">
              <label>
                <span class="text-danger me-1">*</span>
                <span>Phòng ban:</span>
              </label>
            </div>

            <div class="col-12 col-sm-5">
              <a-input placeholder="Phòng ban" allow-clear />
            </div>
          </div>

          <div class="row mb-3">
            <div class="col-12 col-sm-3 text-start text-sm-end">
              <label>
                <span class="text-danger me-1">*</span>
                <span>Mô tả:</span>
              </label>
            </div>

            <div class="col-12 col-sm-5">
              <a-input placeholder="Mô tả" allow-clear />
            </div>
          </div>
        </div>
      </div>

      <div class="row">
        <div class="col-12 d-grid d-sm-flex justify-content-sm-end mx-auto">
          <a-button class="me-0 me-sm-2 mb-3 mb-sm-0">
            <router-link :to="{ name: 'admin-areas' }">
              <span>Hủy</span>
            </router-link>
          </a-button>

          <a-button type="primary">
            <span>Lưu</span>
          </a-button>
        </div>
      </div>
    </a-card>
  </form>
</template>

<script>
import { defineComponent } from "vue";
import { useRouter } from "vue-router";
import { useMenu } from "../../../stores/user-menu.js";

export default defineComponent({
  setup() {
    useMenu().onSelectedKeys(["admin-areas"]);
    const router = useRouter();
    const areas = reactive({
      name: "",
      description: "",
    });
    const getAreaCreate = () => {
      axios
        .get("/api/area/create")
        .then((response) => {
          users_status.value = response.data.users_status;
          departments.value = response.data.departments;
        })
        .catch((error) => {
          console.log(error);
        });
    };
    const createArea = () => {
      axios
        .post("/api/area", areas)
        .then((response) => {
          if (response.status == 200) {
            message.success("Tạo mới thành công!");
            router.push({ name: "admin-areas" });
          }
        })
        .catch((error) => {
          errors.value = error.response.data.errors;
        });
    };
    getAreaCreate();
    return {
      ...toRefs(areas),
      errors,
      filterOption,
      createArea,
    };
  },
});
</script>
