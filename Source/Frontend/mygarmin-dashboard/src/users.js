import * as React from "react";
import { List, Datagrid, TextField, SimpleForm, TextInput, required } from 'react-admin';
import { Edit } from 'react-admin';
import { Create } from 'react-admin';

export const UserList = props => (
    <List {...props}>
        <Datagrid rowClick="edit">
            <TextField source="id" />
            <TextField source="username" />
            <TextField source="firstname" />
            <TextField source="lastname" />
        </Datagrid>
    </List>
);

export const UserEdit = (props) => (
    <Edit {...props}>
        <SimpleForm>
            <TextInput disabled label="Id" source="id" />
            <TextInput disabled label="Username" source="username" />
            <TextInput label="Firstname" source="firstname" validate={required()} />
            <TextInput label="Lastname" source="lastname" validate={required()} />
        </SimpleForm>
    </Edit>
);

export const UserCreate = props => (
    <Create {...props}>
        <SimpleForm>
            <TextInput source="username" validate={required()} />
            <TextInput source="firstname" validate={required()} />
            <TextInput source="lastname" validate={required()} />
        </SimpleForm>
    </Create>
);