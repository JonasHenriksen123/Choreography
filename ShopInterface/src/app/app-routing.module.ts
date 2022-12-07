import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { MaintenanceComponent } from './maintenance/maintenance.component';
import { ShopComponent } from './shop/shop.component';

const routes: Routes = [
  { path: '', component: MainComponent },
  { path: 'shop', component: ShopComponent },
  { path: 'maintenance', component: MaintenanceComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
