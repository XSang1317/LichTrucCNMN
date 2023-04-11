import Home from "../components/Home.vue";
import Login from "../components/Login/AuthModal.vue";
import Register from "../components/Register.vue";

const routes = [
    {
        path: "/",
        name: "Home",
        component: Home,
    },
    {
        path: "/Login",
        name: "Login",
        component: Login,
    },
    {
        path: "/Register",
        name: "Register",
        component: Register,
    },
    
];

export default routes;