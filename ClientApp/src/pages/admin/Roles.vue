<template>
  <div id="wrapper">
    <div id="content-wrapper">
      <div class="container-fluid">
        <div class="row">
          <div class="col-4 pr-1">
            <form
              @submit.prevent="addItem"
              class="form-inline"
              style="height: 40px; max-height: 40px"
            >
              <a-button
                class="btn-sm border-0 btn-info mb-1 mr-2"
                style="float: right"
                shape="round"
                type="primary"
                ><span><i class="fa-solid fa-user-plus" /> Add User</span></a-button
              >
              <a-input
                class="mr-2 h-auto pt-0 pb-0"
                style="flex-grow: 1; width: 1px"
                v-model="added_item_name"
                aria-placeholder="Group Name"
              />
            </form>
            <a-table style striped id="my-tables" head-type bordered :dataSource="dataSource" :columns="columns">
              <template>
                <template v-if="column.dataIndex === 'name'">
                  <div class="editable-cell"></div>
                </template>
              </template>
            </a-table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { useMenu } from "../../stores/user-menu";
export default {
  setup() {
    useMenu().onSelectedKeys(["admin-roles"]);
  },
};
</script>
