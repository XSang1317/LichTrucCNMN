export const initialise = (state, payload) => {
    Object.assign(state, payload);
  };
  export const loginSuccess = (state, payload) => {
    state.auth = payload;
  };
  
  export const logout = (state) => {
    state.auth = null;
  };
  
  