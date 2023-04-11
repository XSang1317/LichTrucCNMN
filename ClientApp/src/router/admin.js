import admin1 from "../pages/admin/admin.vue"
import staffs from "../pages/admin/Staffs.vue"
import roles from "../pages/admin/Roles.vue"
import areas from "../pages/admin/Areas.vue"
import shifts from "../pages/admin/Shifts.vue"
import shiftstype from "../pages/admin/Shiftstype.vue"

const admin = [
    {
        path: "/admin",
        component: admin1,
        children: [
            {
                path: "staffs",
                name: "admin-staffs",
                component: staffs, 
            },

            {
                path: "roles",
                name: "admin-roles",
                component: roles
            },
            {
                path: "shifts",
                name: "admin-shifts",
                component: shifts
            },
            {
                path: "type",
                name: "admin-type",
                component: shiftstype
            },

            {
                path: "areas",
                name: "admin-areas",
                component: areas
            }
        ]
    }
];

export default admin;