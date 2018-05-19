import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dependencies',
  templateUrl: './dependencies.component.html'
})
export class DependenciesComponent implements OnInit {

  technologies: Technology[] = [
    {
      name: 'Frameworks and database',
      dependencies: frameworksAndDatabase
    },
    {
      name: 'ASP.NET Core 2.0 WebAPI dependencies',
      dependencies: WebAPIDependencies
    },
    {
      name: 'Angular dependencies',
      dependencies: angularDependencies
    }
  ]

  constructor() { }

  ngOnInit() {
  }

}

interface Technology {
  name: string;
  dependencies: Dependency[]
}

interface Dependency {
  name: string;
  version: string;
  url: string;
}

const frameworksAndDatabase: Dependency[] = [
  { 
    name: 'ASP.NET Core 2.0 Web API', 
    version: '', 
    url: 'https://github.com/aspnet/Mvc'
  },
  {
    name: 'Angular',
    version: '5.2.7',
    url: 'https://angular.io/'
  },
  {
    name: 'MySQL',
    version: '5.7.21',
    url: 'https://www.mysql.com'
  }
]

const angularDependencies: Dependency[] = [
  {
    name: 'Bootstrap',
    version: '4.0.0',
    url: 'https://getbootstrap.com/'
  },
  {
    name: 'ng-bootstrap',
    version: '1.0.0',
    url: 'https://github.com/ng-bootstrap/ng-bootstrap'
  },
  {
    name: 'Font Awesome',
    version: '4.7.0',
    url: 'https://fontawesome.com/'
  },
  {
    name: 'angular-font-awesome',
    version: '3.1.2',
    url: 'https://github.com/baruchvlz/angular-font-awesome'
  },
  {
    name: 'Froala WYSIWYG',
    version: '2.8.1',
    url: 'https://www.npmjs.com/package/angular-froala-wysiwyg'
  },
  {
    name: 'jwt-decode',
    version: '2.2.0',
    url: 'https://github.com/auth0/jwt-decode'
  },
  {
    name: 'ngx-chips',
    version: '1.8.0',
    url: 'https://github.com/Gbuomprisco/ngx-chips'
  },
  {
    name: 'ngx-infinite-scroll',
    version: '0.8.4',
    url: 'https://github.com/orizens/ngx-infinite-scroll'
  },
  {
    name: 'ngx-moment',
    version: '2.0.0',
    url: 'https://github.com/urish/ngx-moment'
  },
  {
    name: 'ngx-errors',
    version: '1.4.0',
    url: 'https://github.com/UltimateAngular/ngx-errors'
  },
  {
    name: 'ngx-progressbar',
    version: '4.3.0',
    url: 'https://github.com/MurhafSousli/ngx-progressbar'
  }
]

const WebAPIDependencies: Dependency[] = [
  {
    name: 'Pomelo.EntityFrameworkCore.MySql',
    version: '2.0.1',
    url: 'https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql'
  },
  {
    name: 'NLog.Web.AspNetCore',
    version: '4.5.0',
    url: 'https://github.com/NLog/NLog.Web'
  },
  {
    name: 'SendGrid',
    version: '9.9.0',
    url: 'https://sendgrid.com/'
  },
  {
    name: 'Swashbuckle.AspNetCore',
    version: '2.2.0',
    url: 'https://github.com/domaindrivendev/Swashbuckle.AspNetCore'
  }
]