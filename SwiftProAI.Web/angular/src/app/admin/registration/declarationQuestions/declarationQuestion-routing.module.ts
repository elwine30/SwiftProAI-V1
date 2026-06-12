import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {DeclarationQuestionsComponent} from './declarationQuestions.component';



const routes: Routes = [
    {
        path: '',
        component: DeclarationQuestionsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DeclarationQuestionRoutingModule {
}
