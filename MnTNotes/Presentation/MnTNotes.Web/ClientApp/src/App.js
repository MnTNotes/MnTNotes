import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchNote } from './components/FetchNote';
import { AddNote } from './components/AddNote';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <AuthorizeRoute path='/fetch-note' component={FetchNote} />
                <AuthorizeRoute path='/add-note' component={AddNote} />
                <AuthorizeRoute path='/note/edit/:noteid' component={AddNote} />
                <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
            </Layout>
        );
    }
}