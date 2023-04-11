import admin1 from "../pages/admin/admin.vue"
import staffs from "../pages/admin/staff/index.vue"
import roles from "../pages/admin/role/Roles.vue"
import areas from "../pages/admin/areas/index.vue"
import shifts from "../pages/admin/Shifts.vue"
import shiftstype from "../pages/admin/Shiftstype.vue"
import permissions from "../pages/admin/Permission.vue"
const admin = [
    {
        path: "/admin",
        component: admin1,
        children: [
            /* Quản lý nhân viên */
            {
                path: "staffs",
                name: "admin-staffs",
                component: staffs, 
            },
           
            /* Quản lý ca làm */
            {
                path: "shifts",
                name: "admin-shifts",
                component: shifts
            },
            /* Quản lý loại ca */
            {
                path: "type",
                name: "admin-type",
                component: shiftstype
            },
            /* Quản lý khu vực */
            {
                path: "areas",
                name: "admin-areas",
                component: areas
            },
            {
                path: "areas/create",
                name: "admin-areas-create",
                component: () => import ("../pages/admin/areas/create.vue")
            },
             /* Quản lý quyền */
             {
                path: "roles",
                name: "admin-roles",
                component: roles
            },
            {
                path: "role/create",
                name: "admin-role-create",
                component: () => import ("../pages/admin/role/create.vue")
            },

            {
                path:"permissions",
                name: "admin-permissions",
                component: permissions
            }
        ]
    }
];

export default admin;