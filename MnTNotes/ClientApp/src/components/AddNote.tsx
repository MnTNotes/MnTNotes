import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { NoteData } from './FetchNote';
import authService from './api-authorization/AuthorizeService'

import { Button, Form, FormGroup, Label, Input, ButtonGroup } from 'reactstrap';

import { EditorState, convertToRaw, convertFromHTML, ContentState } from 'draft-js';
import { ContentBlock, Editor } from 'react-draft-wysiwyg';
import "react-draft-wysiwyg/dist/react-draft-wysiwyg.css";
import draftToHtml from 'draftjs-to-html';

interface RouteComponetPath {
    path?: string
}

interface NoteContainerProps {
    noteid: number
    match: any
    history: any
}

interface AddNoteDataState {
    title: string;
    loading: boolean;
    noteData: NoteData;
    editorState: EditorState;
    authtoken: any;
}

export class AddNote extends React.Component<NoteContainerProps | RouteComponentProps<RouteComponetPath>, AddNoteDataState> {
    constructor(props: NoteContainerProps & RouteComponentProps) {
        super(props);

        this.state = { title: "", authtoken: "", loading: true, noteData: new NoteData(), editorState: EditorState.createEmpty() };
        var noteid = this.props.match.params["noteid"];
        if (noteid > 0) {
            this.editgetnote(noteid);
        }
        else {
            this.state = { title: "New Note Title", authtoken: "", loading: false, noteData: new NoteData(), editorState: EditorState.createEmpty() };
        }

        this.handleSave = this.handleSave.bind(this);
        this.handleCancel = this.handleCancel.bind(this);

        this.uploadImageCallBack = this.uploadImageCallBack.bind(this);
    }

    componentDidMount() {
        this.getToken();
    }

    private onEditorChange = (editorState: any) => this.setState({ editorState });

    private uploadImageCallBack(file: any) {

        return new Promise(
            (resolve: any, reject: any) => {
                const xhr = new XMLHttpRequest();
                xhr.open('POST', '/Image');
                xhr.setRequestHeader('Authorization', this.state.authtoken);
                const data = new FormData();
                data.append('image', file);
                xhr.send(data);
                xhr.addEventListener('load', () => {
                    const response = JSON.parse(xhr.responseText);
                    console.log(response)
                    resolve(response);
                });
                xhr.addEventListener('error', () => {
                    const error = JSON.parse(xhr.responseText);
                    console.log(error)
                    reject(error);
                });
            }
        );
    }

    public render() {

        let contents = (this.state.loading || !this.state.authtoken)
            ? <p><em>Loading...</em></p>
            : this.renderCreateForm();

        return <div>
            <h3>New Note</h3>
            <p>Adding new note...</p>
            <hr />
            {contents}
        </div>;

    }

    private async editgetnote(noteid: any) {
        const token = await authService.getAccessToken();
        const response = await fetch('Note/' + noteid, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const responsejson = await (await response.json() as Promise<NoteData>);
        this.setState({
            title: "Edit", loading: false, noteData: responsejson,
            editorState: EditorState.createWithContent(ContentState.createFromBlockArray(convertFromHTML(responsejson.content).contentBlocks))
        });

    }

    // This will handle the submit form event.
    private handleSave(event: any) {
        event.preventDefault();
        const data = new FormData(event.target);

        // PUT request
        if (this.state.noteData.id) {
            this.putNoteData(this.state.noteData.id, data);
        }

        // POST request
        else {
            this.postNoteData(data);
        }
    }

    private async postNoteData(data: any) {
        const token = await authService.getAccessToken();
        const response = await fetch('Note/', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` },
            method: 'POST',
            body: data
        });
        //const responsejson = await response.json();
        await response.json();
        this.props.history.push("/fetch-note");
    }

    private async putNoteData(id: number, data: any) {
        const token = await authService.getAccessToken();
        const response = await fetch('Note/' + id, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` },
            method: 'PUT',
            body: data
        });
        //const responsejson = await response.json();
        await response.json();
        this.props.history.push("/fetch-note");
    }

    // This will handle Cancel button click event.
    private handleCancel(e: any) {
        e.preventDefault();
        this.props.history.push("/fetch-note");
    }

    // Returns the HTML Form to the render() method.
    private renderCreateForm() {
        const { editorState } = this.state;
        return (

            <div>

                <Form onSubmit={this.handleSave}>
                    <FormGroup>
                        <div className="form-group row" >
                            <input type="hidden" name="id" value={this.state.noteData.id} />
                        </div>
                    </FormGroup>
                    <FormGroup>
                        <Label for="title">Title</Label>
                        <Input type="text" name="title" id="title" defaultValue={this.state.noteData.title} placeholder="The title goes here..." required />
                    </FormGroup>
                    <FormGroup>
                        <Label>Content</Label>
                        <Editor
                            wrapperStyle={{
                                border: '1px solid #ced4da', borderRadius: '0.25rem'
                            }}
                            editorState={editorState}
                            placeholder="The content goes here..."
                            toolbar={{
                                inline: { inDropdown: true },
                                list: { inDropdown: true },
                                textAlign: { inDropdown: true },
                                link: { inDropdown: true },
                                history: { inDropdown: true },
                                image: { uploadCallback: this.uploadImageCallBack, previewImage: true, alt: { present: false, mandatory: false } },
                            }}
                            onEditorStateChange={this.onEditorChange}
                        />
                    </FormGroup>
                    <FormGroup>
                        <Input
                            hidden readOnly
                            name="content" id="content"
                            type="textarea"
                            value={draftToHtml(convertToRaw(editorState.getCurrentContent()))} />
                    </FormGroup>
                    <ButtonGroup>
                        <Button type="submit" color="success">Save</Button>
                        <Button color="danger" onClick={this.handleCancel}>Cancel</Button>
                    </ButtonGroup>
                </Form>


                <div
                    style={{
                        border: '1px solid #ced4da', borderRadius: '0.25rem'
                    }}
                    dangerouslySetInnerHTML={{ __html: draftToHtml(convertToRaw(editorState.getCurrentContent())) }}>
                </div>

            </div>
        )
    }


    async getToken() {
        const token = await authService.getAccessToken();
        const userToken = { 'Authorization': `Bearer ${token}` };
        this.setState({ authtoken: userToken.Authorization, loading: false });
    }

}