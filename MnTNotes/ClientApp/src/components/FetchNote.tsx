import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { RouteComponentProps } from 'react-router';
import authService from './api-authorization/AuthorizeService'

import { Container, Row, Col, Card, CardHeader, CardBody, CardText, CardFooter, Button, ButtonGroup } from 'reactstrap';

interface RouteComponetPath {
    path?: string
}

interface FetchNoteContainerProps {
    noteid: number
    match: any
    history: any
}

export class NoteData {
    id: number = 0;
    title: string = "";
    content: string = "";
}

interface FetchNoteDataState {
    noteList: NoteData[];
    loading: boolean;
}

export class FetchNote extends React.Component<FetchNoteContainerProps | RouteComponentProps<RouteComponetPath>, FetchNoteDataState> {
    static displayName = FetchNote.name;

    constructor(props: any) {
        super(props);
        this.state = { noteList: [], loading: true };

        this.handleDelete = this.handleDelete.bind(this);
        this.handleEdit = this.handleEdit.bind(this);
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderNotesTable(this.state.noteList);

        return (
            <div>
                <h1 id="tableLabel" >Notes <NavLink className="btn btn-sm btn-success" to="/add-note">Add Note</NavLink></h1>
                <p>You can add, edit and delete your notes...</p>
                <hr />
                {contents}
            </div>
        );
    }

    componentDidMount() {
        this.populateNoteData();
    }

    private async handleDelete(id: number) {
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

    private handleEdit(id: number) {
        this.props.history.push("/note/edit/" + id);
    }

    renderNotesTable(noteList: NoteData[]) {
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
        const response = await fetch('note', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const data = await response.json();
        this.setState({ noteList: data, loading: false });
    }

}
