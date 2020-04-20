import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginGuard } from 'src/app/core/guards/login.guard';
import { PositionManageComponent } from './position-manage/position-manage.component';
import { AppCommonModule } from '../app-common/app-common.module';
import { DepartmentTreeComponent } from './department-tree/department-tree.component';
import { DepartmentManageComponent } from './department-manage/department-manage.component';
import { EmployeeComponent } from './employee/employee.component';



@NgModule({
  declarations: [
    PositionManageComponent,
    DepartmentTreeComponent,
    DepartmentManageComponent,
    EmployeeComponent,
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    RouterModule.forChild([
      { path: 'position', component: PositionManageComponent, canActivate: [LoginGuard] },
      { path: 'department', component: DepartmentManageComponent, canActivate: [LoginGuard] },
      { path: 'employee', component: EmployeeComponent, canActivate: [LoginGuard] },
    ])
  ]
})
export class GroupManageModule { }
