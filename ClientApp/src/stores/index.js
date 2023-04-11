import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

//import * as actions from "./actions";
import * as mutations from "./mutations";
import * as getters from "./getters";
import * as usermenu from "./user-menu";
const store = new Vuex.Store({
  strict: true,
  //actions,
  mutations,
  getters,
  usermenu,
  state: {    
    auth: null,
  }
});

store.subscribe((mutation, state) => {
  localStorage.setItem("store", JSON.stringify(state));
});

export default store;
