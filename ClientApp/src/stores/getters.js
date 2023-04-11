export const isAuthenticated = (state) => {
    return state.auth !== null;
  };
  
  export const hasPermission = (state, getters) => (permission) => {
    const result =
      getters.isAuthenticated && state.auth.permissions.indexOf(permission) > -1;
    return result;
  };
  
  export const departments = (state) => {
    return state.auth.departments;
  };
  
  export const department = (state) => {
      return state.auth.departments.find(x => x.id === state.auth.departmentId);
    };
  
    export const departmentid = (state) => {
      return state.auth.departmentId;
    };
  
  