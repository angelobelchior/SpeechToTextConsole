# SpeechToTextConsole

**Speech To Text Console** é um projeto para converter áudio em texto.

Esse projeto utiliza o Serviço Cognitivo da Microsoft [**Bing Speech API**](https://azure.microsoft.com/en-us/services/cognitive-services/speech/)

---

## Como testar

Você pode usar o console passando por parâmetro o caminho do arquivo *.wav para a transcrição.

Caso queira efetuar um teste, disponibilizei um arquivo de áudio.

O aúdio em questão é um episódio do podcast [**Nerdologia**](https://jovemnerd.com.br/nerdologia/). Recomendo muito, mas muito mesmo, acompanharem. **É simplesmente SENSACIONAL**.

Execute o projeto em modo **DEBUG**, e agurdade o arquivo de texto com a transcrição ser gerado.

Você pode criar um *.exe e utilizar como um Job no Windows ou até mesmo na núvem.

---

## Chaves de Segurança para o Serviço

Você precisará informar a chave de segurança para poder utilizar os serviço.
No arquivo [app.config](https://github.com/angelobelchior/SpeechToTextConsole/blob/master/SpeechToTextConsole/app.config) você vai precisar informar três valores:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="SpeechToTextKey" value="" />
    <add key="Language" value="" />
    <add key="OutputFolder" value="" />
  </appSettings>
</configuration>
```

* **SpeechToTextKey**: [Clique Aqui para Obter uma chave](https://azure.microsoft.com/en-us/try/cognitive-services/?api=speech-api)
* **Language**: Idioma. (pt-br, en-us, etc)
* **OutputFolder**: Caminho onde será gerado o arquivo *.txt contendo a transcrição do áudio.

**Você vai precisar ter uma conta no Microsoft Azure. Você pode criar gratuitamente.**

Para maiores informações:

* [Crie sua conta gratuita do Azure hoje mesmo](https://azure.microsoft.com/pt-br/free/)
* [Perguntas frequentes sobre a Conta Gratuita do Azure](https://azure.microsoft.com/pt-br/free/free-account-faq/)
* [Tutorial Criando uma conta no Microsoft Azure e utilizando os créditos gratuitos](https://www.youtube.com/watch?v=tAixhiHmphA)

**Todos esses serviços disponibilizam uma camada gratuita para testes. Isso significa que você não precisará gastar nenhum centavo para usufruir dessas tecnologias.**

Você também pode optar por uma camada paga. O custo varia, mas no geral, é bem baixo.

---
