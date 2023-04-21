<template>
  <form @submit.prevent="createArea()">
    <a-card title="Thêm khu vực" style="width: 100%">
      <div class="row mb-3">
        <div class="col-12 col-sm-8">
          <!-- Name -->
          <div class="row mb-3">
            <div class="col-12 col-sm-3 text-start text-sm-end">
              <label>
                <span class="text-danger me-1">*</span>
                <span
                  :class="{
                    'text-danger': errors.Name,
                  }"
                >
                  Tên Khu Vực:
                </span>
              </label>
            </div>

            <div class="col-12 col-sm-5">
              <a-input
                placeholder="Tên Khu Vực"
                allow-clear
                v-model:value="Name"
                :class="{
                  'input-danger': errors.Name,
                }"
              />

              <div class="w-100"></div>

              <small v-if="errors.Name" class="text-danger">{{ errors.Name[0] }}</small>
            </div>
          </div>
          <!-- Description -->
          <div class="row mb-3">
            <div class="col-12 col-sm-3 text-start text-sm-end">
              <label>
                <span class="text-danger me-1">*</span>
                <span
                  :class="{
                    'text-danger': errors.Description,
                  }"
                >
                  Mô tả:
                </span>
              </label>
            </div>

            <div class="col-12 col-sm-5">
              <a-input
                placeholder="Description"
                allow-clear
                v-model:value="Description"
                :class="{
                  'input-danger': errors.Description,
                }"
              />

              <div class="w-100"></div>

              <small v-if="errors.Description" class="text-danger">{{
                errors.Description[0]
              }}</small>
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

          <a-button type="primary" html-type="submit">
            <span>Lưu</span>
          </a-button>
        </div>
      </div>
    </a-card>
  </form>
</template>

<script>
import { defineComponent, reactive, ref } from "vue";
import { useRouter } from "vue-router";
import { useMenu } from "../../../stores/user-menu.js";

export default defineComponent({
  setup() {
    useMenu().onSelectedKeys(["admin-areas"]);
    const router = useRouter();
    const newAreas = reactive({
      Name: "",
      Description: "",
      //CreatedAt: "",
    });
    const errors = ref({});
    const filterOption = (input, option) => {
      return option.label.toLowerCase().indexOf(input.toLowerCase()) >= 0;
    };
    const createArea=()=> {
      axios
        .post("/api/area", newAreas)
        .then((response) => {
          if(response){
           message.success("Tạo mới thành công!");
            router.push({ name: "admin-areas" });
          }
        })
        .catch((error) => {
          console.log(error);
        });
    }
    return{
      errors,
      filterOption,
      createArea,
    }
  },
  /* data() {
    return {
      newArea: {
        name: "",
        description: "",
      },
    };
  }, */
  methods: {

  },
});
</script>
