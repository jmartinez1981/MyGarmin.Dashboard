import * as React from "react";
import { List, Datagrid, TextField, SimpleForm, PasswordInput, TextInput, required } from 'react-admin';
import { Edit } from 'react-admin';
import { Create } from 'react-admin';

export const UserList = props => (
    <List {...props}>
        <Datagrid rowClick="edit">
            <TextField label="Username" source="id" />
            <TextField source="firstname" />
            <TextField source="lastname" />
        </Datagrid>
    </List>
);

export const UserEdit = (props) => (
    <Edit {...props}>
        <SimpleForm>
            <TextInput disabled label="Username" source="id" />
            <TextInput label="Firstname" source="firstname" validate={required()} />
            <TextInput label="Lastname" source="lastname" validate={required()} />
        </SimpleForm>
    </Edit>
);

export const UserCreate = props => (
    <Create {...props}>
        <SimpleForm>
            <TextInput label="Username" source="id" validate={required()} />
            <TextInput source="firstname" validate={required()} />
            <TextInput source="lastname" validate={required()} />
            <PasswordInput source="password" type="password" validate={required()} />
        </SimpleForm>
    </Create>
);