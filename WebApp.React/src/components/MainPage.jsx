import { useState } from 'react'
import PageButton from './PageButton.jsx'
import TodoListTable from '../todos/TodoListTable.jsx'
import { LogOutButton } from './LogOutButton.jsx'
import { Register } from '../auth/Register.jsx'
import TaskDescription from '../tasks/TaskDescription.jsx'
import TaskList from '../tasks/TaskList.jsx'

function MainPage() {
    const [todoListId, setTodoListId] = useState(null);
    const [taskId, setTaskId] = useState(-1);
    const [editedTaskId, setEditedTaskId] = useState(0);

    return (
        <div
            className="row"
        >
            <TodoListTable
                setTodoListId={setTodoListId}
            />

            <TaskList
                todoListId={todoListId}
                setTaskId={setTaskId}
                editedTaskId={editedTaskId}
            />

            <TaskDescription
                todoListId={todoListId}
                taskId={taskId}
                setEditedTaskId={setEditedTaskId}
            />

            <LogOutButton></LogOutButton>
        </div>
    )
}

export default MainPage