import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';
import { ApplicationPaths } from './api-authorization/ApiAuthorizationConstants';
import authService from './api-authorization/AuthorizeService';
import { Container, Row, Col, Card, CardHeader, CardBody, CardText, CardFooter, Button, ButtonGroup } from 'reactstrap';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
            isAuthenticated: false,
            userName: null,
            noteList: [],
            loading: true
        };

        this.handleDelete = this.handleDelete.bind(this);
        this.handleEdit = this.handleEdit.bind(this);
    }

    componentDidMount() {
        this._subscription = authService.subscribe(() => this.populateState());
        this.populateState();
        this.populateNoteData();
    }

    componentWillUnmount() {
        authService.unsubscribe(this._subscription);
    }

    async populateState() {
        const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()])
        this.setState({
            isAuthenticated,
            userName: user && user.name
        });
    }

    render() {
        const { isAuthenticated, userName } = this.state;
        if (!isAuthenticated) {
            const registerPath = `${ApplicationPaths.Register}`;
            const loginPath = `${ApplicationPaths.Login}`;
            return this.anonymousView(registerPath, loginPath);
        } else {
            return this.authenticatedView(userName);
        }
    }

    authenticatedView(userName) {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderNotesTable(this.state.noteList);

        return (
            <div>
                <h1 id="tableLabel" >User Dashboard <NavLink className="btn btn-sm btn-success" to="/add-note">Add Note</NavLink></h1>
                <p>
                    {userName}
                </p>
                <hr />
                {contents}
            </div>
        );
    }

    anonymousView(registerPath, loginPath) {
        return (
            <div>
                <h1>Another Note App!</h1>
                <p>Note that from:</p>
                <ul>
                    <li><strong>Web: </strong><a href='https://get.asp.net/'>Note Web</a></li>
                    <li><strong>Mobile: </strong><a href='https://facebook.github.io/react/'>Note Mobile</a></li>
                </ul>
                <p><code>Note</code> any think! <code>Note</code> anything!</p>
                <p>Just Register or Login!</p>
                <p>
                    <a className="text-light btn btn-success" href={registerPath}>Register</a>
                </p>
                <p>
                    <a className="text-light btn btn-success" href={loginPath}>Login</a>
                </p>
            </div>
        );
    }

    async handleDelete(id) {
        const token = await authService.getAccessToken();
        await fetch('Note/' + id, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` },
            method: 'DELETE'
        });
        this.setState(
            {
                noteList: this.state.noteList.filter((rec) => {
                    return (rec.id !== id);
                })
            });
    }

    handleEdit(id) {
        this.props.history.push("/note/edit/" + id);
    }

    renderNotesTable(noteList) {
        return (
            <Container>
                <Row>
                    {noteList.map(note =>
                        <Col md="4" key={note.id}>
                            <Card style={{ margin: 10 }}>
                                <CardHeader>{note.title}</CardHeader>
                                <CardBody style={{ height: 150 }}>
                                    <CardText
                                        dangerouslySetInnerHTML={{ __html: (note.content) ? (note.content.length > 100 ? note.content.substring(0, 100) + " ..." : note.content) : "" }}>
                                    </CardText>
                                </CardBody>
                                <CardFooter>
                                    <ButtonGroup>
                                        <Button color="success" onClick={(id) => this.handleEdit(note.id)}>Show & Edit</Button>
                                        <Button color="danger" onClick={(id) => this.handleDelete(note.id)}>Delete</Button>
                                    </ButtonGroup>
                                </CardFooter>
                            </Card>
                        </Col>
                    )}
                </Row>
            </Container>
        );
    }

    async populateNoteData() {
        const token = await authService.getAccessToken();
        if (token !== null) {
            const response = await fetch('note', {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });

            const data = await response.json();

            console.log(data);

            this.setState({ noteList: data, loading: false });
        }
    }
}