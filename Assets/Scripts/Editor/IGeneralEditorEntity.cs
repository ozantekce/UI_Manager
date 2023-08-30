/*
public abstract class IGeneralEditorEntity<E> where E : class
{

    public void Enable(GeneralEditorHandler editor)
    {
        E e = Convert(editor);
        if (e == null) return;
        OnEnable(editor,e);
    }

    public void InspectorGUI(GeneralEditorHandler editor)
    {
        E e = Convert(editor);
        if (e == null) return;
        OnInspectorGUI(editor, e);
    }

    protected abstract void OnEnable(GeneralEditorHandler editor, E entity);

    protected abstract void OnInspectorGUI(GeneralEditorHandler editor, E entity);

    private E Convert(GeneralEditorHandler editor)
    {
        E e = editor.target as E;
        return e;
    }




}
*/